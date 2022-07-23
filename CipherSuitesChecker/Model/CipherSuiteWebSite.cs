using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CipherSuitesChecker.Model
{
    public class CipherSuiteWebSite
    {
        public async Task<IEnumerable<CipherSuite>> RequestCipherSuites()
        {
            var cipherSuites = new List<CipherSuite>();
            var client = new HttpClient();
            var reply = await client.GetStringAsync("https://ciphersuite.info/api/cs");

            var jsonUtf8Stream = new MemoryStream(Encoding.UTF8.GetBytes(reply));
            var json = await JsonSerializer.DeserializeAsync<object>(jsonUtf8Stream);
            if (json == null)
                return new List<CipherSuite>();
            var jsonElement = (JsonElement)json;
            var cipherSuitesArray = jsonElement.GetProperty("ciphersuites");
            var cipherSuitesArrayEnumerator = cipherSuitesArray.EnumerateArray();
            
            foreach (var cipherSuiteArrayElement in cipherSuitesArrayEnumerator)
            {
                var cipherSuiteObjectEnumerator = cipherSuiteArrayElement.EnumerateObject();
                cipherSuiteObjectEnumerator.MoveNext();
                var cipherSuiteKeyObj = cipherSuiteObjectEnumerator.Current;
                var cipherSuiteKey = cipherSuiteKeyObj.Name;
                var cipherSuiteObj = cipherSuiteKeyObj.Value;

                var gnuTlsName = cipherSuiteObj.GetProperty("gnutls_name").GetString() ?? "";
                var openSslName = cipherSuiteObj.GetProperty("openssl_name").GetString() ?? "";
                var hexByte1 = cipherSuiteObj.GetProperty("hex_byte_1").GetString() ?? "";
                var hexByte2 = cipherSuiteObj.GetProperty("hex_byte_2").GetString() ?? "";
                var protocol = cipherSuiteObj.GetProperty("protocol_version").GetString() ?? "";
                var protocolsArrayEnumerator = cipherSuiteObj.GetProperty("tls_version").EnumerateArray();
                var protocols = protocolsArrayEnumerator.Select(e => e.GetString() ?? "");
                var kexAlgorithm = cipherSuiteObj.GetProperty("kex_algorithm").GetString() ?? "";
                var authAlgorithm = cipherSuiteObj.GetProperty("auth_algorithm").GetString() ?? "";
                var encAlgorithm = cipherSuiteObj.GetProperty("enc_algorithm").GetString() ?? "";
                var hashAlgorithm = cipherSuiteObj.GetProperty("hash_algorithm").GetString() ?? "";
                var security = cipherSuiteObj.GetProperty("security").GetString() ?? "";

                var cipherSuite = new CipherSuite();
                cipherSuite.Name = cipherSuiteKey;
                cipherSuite.GnuTlsName = gnuTlsName;
                cipherSuite.OpenSslName = openSslName;
                cipherSuite.HexByte1 = hexByte1;
                cipherSuite.HexByte2 = hexByte2;
                cipherSuite.Protocol = protocol;
                cipherSuite.Protocols = new ObservableCollection<string>(protocols);
                cipherSuite.KeyExchangeAlgorithm = kexAlgorithm;
                cipherSuite.AuthenticationAlgorithm = authAlgorithm;
                cipherSuite.EncryptionAlgorithm = encAlgorithm;
                cipherSuite.HashAlgorithm = hashAlgorithm;
                cipherSuite.Security = security;
                cipherSuites.Add(cipherSuite);
            }

            return cipherSuites;
        }
    }
}
