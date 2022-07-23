using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CipherSuitesChecker.Model
{
    public class CipherSuite : INotifyPropertyChanged
    {
        private string name;
        private string gnuTlsName;
        private string openSslName;
        private string security;
        private string hashAlgorithm;
        private string authenticationAlgorithm;
        private string encryptionAlgorithm;
        private string keyExchangeAlgorithm;
        private string protocol;
        private ObservableCollection<string> protocols;
        private string hexByte1;
        private string hexByte2;
        private string comment;

        #region Constructors

        public CipherSuite()
        {
            name = "";
            gnuTlsName = "";
            openSslName = "";
            security = "";
            hashAlgorithm = "";
            authenticationAlgorithm = "";
            encryptionAlgorithm = "";
            keyExchangeAlgorithm = "";
            protocol = "";
            protocols = new ObservableCollection<string>();
            hexByte1 = "";
            hexByte2 = "";
            comment = "";
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return name; }
            set
            {
                if (name == value)
                    return;
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string GnuTlsName
        {
            get { return gnuTlsName; }
            set
            {
                if (gnuTlsName == value)
                    return;
                gnuTlsName = value;
                OnPropertyChanged(nameof(GnuTlsName));
            }
        }

        public string OpenSslName
        {
            get { return openSslName; }
            set
            {
                if (openSslName == value)
                    return;
                openSslName = value;
                OnPropertyChanged(nameof(OpenSslName));
            }
        }

        public string Security
        {
            get { return security; }
            set
            {
                if (security == value)
                    return;
                security = value;
                OnPropertyChanged(nameof(Security));
            }
        }

        public string HashAlgorithm
        {
            get { return hashAlgorithm; }
            set
            {
                if (hashAlgorithm == value)
                    return;
                hashAlgorithm = value;
                OnPropertyChanged(nameof(HashAlgorithm));
            }
        }

        public string AuthenticationAlgorithm
        {
            get { return authenticationAlgorithm; }
            set
            {
                if (authenticationAlgorithm == value)
                    return;
                authenticationAlgorithm = value;
                OnPropertyChanged(nameof(AuthenticationAlgorithm));
            }
        }

        public string EncryptionAlgorithm
        {
            get { return encryptionAlgorithm; }
            set
            {
                if (encryptionAlgorithm == value)
                    return;
                encryptionAlgorithm = value;
                OnPropertyChanged(nameof(EncryptionAlgorithm));
            }
        }

        public string KeyExchangeAlgorithm
        {
            get { return keyExchangeAlgorithm; }
            set
            {
                if (keyExchangeAlgorithm == value)
                    return;
                keyExchangeAlgorithm = value;
                OnPropertyChanged(nameof(KeyExchangeAlgorithm));
            }
        }

        public string Protocol
        {
            get { return protocol; }
            set
            {
                if (protocol == value)
                    return;
                protocol = value;
                OnPropertyChanged(nameof(Protocol));
            }
        }

        public ObservableCollection<string> Protocols
        {
            get { return protocols; }
            set
            {
                if (protocols == value)
                    return;
                protocols = value;
                OnPropertyChanged(nameof(Protocols));
            }
        }

        public string HexByte1
        {
            get { return hexByte1; }
            set
            {
                if (hexByte1 == value)
                    return;
                hexByte1 = value;
                OnPropertyChanged(nameof(HexByte1));
            }
        }

        public string HexByte2
        {
            get { return hexByte2; }
            set
            {
                if (hexByte2 == value)
                    return;
                hexByte2 = value;
                OnPropertyChanged(nameof(HexByte2));
            }
        }

        public string Comment
        {
            get { return comment; }
            set
            {
                if (comment == value)
                    return;
                comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }

    public class CipherSuiteProtocols
    {
        public const string Tls1 = "TLS1.0";
        public const string Tls11 = "TLS1.1";
        public const string Tls12 = "TLS1.2";
        public const string Tls13 = "TLS1.3";
    }

    public class CipherSuiteSecurity
    {
        public const string Recommended = "recommended";
        public const string Secure = "secure";
        public const string Weak = "weak";
        public const string Insecure = "insecure";
    }
}
