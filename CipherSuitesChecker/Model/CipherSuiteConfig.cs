using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace CipherSuitesChecker.Model
{
    public class CipherSuiteConfig
    {
        private readonly XmlDocument xmlDocument;

        public CipherSuiteConfig()
        {
            xmlDocument = new XmlDocument();
        }

        public void LoadFile(string filePath)
        {
            xmlDocument.Load(filePath);
        }

        public void LoadXml(string xml)
        {
            xmlDocument.LoadXml(xml);
        }

        public IEnumerable<CipherSuite> CipherSuites
        {
            get
            {
                var cipherSuites = new List<CipherSuite>();
                var cipherSuiteNodes = new List<XmlNode>();
                var selectedNodes = xmlDocument.SelectNodes("/CipherSuites/CipherSuite");
                if (selectedNodes != null)
                    cipherSuiteNodes = selectedNodes.OfType<XmlNode>().ToList();

                foreach (var cipherSuiteNode in cipherSuiteNodes)
                {
                    var name = GetValue(cipherSuiteNode, "Name") ?? "";
                    var gnuTlsName = GetValue(cipherSuiteNode, "GnuTlsName") ?? "";
                    var openSslName = GetValue(cipherSuiteNode, "OpenSslName") ?? "";
                    var security = GetValue(cipherSuiteNode, "Security") ?? "";
                    var hashAlgorithm = GetValue(cipherSuiteNode, "HashAlgorithm") ?? "";
                    var kexAlgorithm = GetValue(cipherSuiteNode, "KexAlgorithm") ?? "";
                    var authAlgorithm = GetValue(cipherSuiteNode, "AuthAlgorithm") ?? "";
                    var encAlgorithm = GetValue(cipherSuiteNode, "EncAlgorithm") ?? "";
                    var protocol = GetValue(cipherSuiteNode, "Protocol") ?? "";
                    var protocols = GetValue(cipherSuiteNode, "Protocols") ?? "";
                    var hexByte1 = GetValue(cipherSuiteNode, "HexByte1") ?? "";
                    var hexByte2 = GetValue(cipherSuiteNode, "HexByte2") ?? "";
                    var comment = GetValue(cipherSuiteNode, "Comment") ?? "";

                    var cipherSuite = new CipherSuite();
                    cipherSuite.Name = name;
                    cipherSuite.GnuTlsName = gnuTlsName;
                    cipherSuite.OpenSslName = openSslName;
                    cipherSuite.Security = security;
                    cipherSuite.HashAlgorithm = hashAlgorithm;
                    cipherSuite.KeyExchangeAlgorithm = kexAlgorithm;
                    cipherSuite.EncryptionAlgorithm = encAlgorithm;
                    cipherSuite.AuthenticationAlgorithm = authAlgorithm;
                    cipherSuite.Protocol = protocol;
                    cipherSuite.Protocols = ParseProtocols(protocols);
                    cipherSuite.HexByte1 = hexByte1;
                    cipherSuite.HexByte2 = hexByte2;
                    cipherSuite.Comment = comment;
                    cipherSuites.Add(cipherSuite);
                }

                return cipherSuites;
            }
            set
            {
                if (xmlDocument.DocumentElement != null)
                    xmlDocument.RemoveChild(xmlDocument.DocumentElement);
                var rootNode = AddNode(xmlDocument, "CipherSuites");
                foreach (var cipherSuite in value)
                {
                    var resourceNode = AddNode(rootNode, "CipherSuite");
                    AddNode(resourceNode, "Name", cipherSuite.Name);
                    AddNode(resourceNode, "GnuTlsName", cipherSuite.GnuTlsName);
                    AddNode(resourceNode, "OpenSslName", cipherSuite.OpenSslName);
                    AddNode(resourceNode, "Security", cipherSuite.Security);
                    AddNode(resourceNode, "HashAlgorithm", cipherSuite.HashAlgorithm);
                    AddNode(resourceNode, "KexAlgorithm", cipherSuite.KeyExchangeAlgorithm);
                    AddNode(resourceNode, "AuthAlgorithm", cipherSuite.AuthenticationAlgorithm);
                    AddNode(resourceNode, "EncAlgorithm", cipherSuite.EncryptionAlgorithm);
                    AddNode(resourceNode, "Protocol", cipherSuite.Protocol);
                    AddNode(resourceNode, "Protocols", ConvertToProtocolsString(cipherSuite.Protocols));
                    AddNode(resourceNode, "HexByte1", cipherSuite.HexByte1);
                    AddNode(resourceNode, "HexByte2", cipherSuite.HexByte2);
                    AddNode(resourceNode, "Comment", cipherSuite.Comment);
                }
            }
        }

        private ObservableCollection<string> ParseProtocols(string protocols)
        {
            var protocolsList = new ObservableCollection<string>();
            var protocolsParts = protocols.Split(',').Where(p => !string.IsNullOrWhiteSpace(p)).Select(p => p.Trim());
            protocolsList = new ObservableCollection<string>(protocolsParts);
            return protocolsList;
        }

        private string ConvertToProtocolsString(ObservableCollection<string> protocols)
        {
            return string.Join(",", protocols);
        }

        public void Save(string filePath)
        {
            xmlDocument.Save(filePath);
        }

        public void Save(StringBuilder stringBuilder)
        {
            var memoryStream = new MemoryStream();
            xmlDocument.Save(memoryStream);
            var bytes = memoryStream.ToArray();
            var encoding = Encoding.UTF8;
            var str = encoding.GetString(bytes);
            stringBuilder.AppendLine(str);
        }

        private static XmlNode AddNode(XmlNode parentNode, string name)
        {
            return AddNode(parentNode, name, "");
        }

        private static XmlNode AddNode(XmlNode parentNode, string name, string value)
        {
            var ownerDocument = parentNode.OwnerDocument;
            if (ownerDocument == null)
                ownerDocument = parentNode as XmlDocument;
            if (ownerDocument == null)
                throw new Exception("The node does not specify the owner document.");
            var element = ownerDocument.CreateElement(name);
            var addedNode = parentNode.AppendChild(element);
            if (addedNode == null)
                throw new Exception("The child node has not been created.");
            if (!string.IsNullOrEmpty(value))
                addedNode.InnerText = value;
            return addedNode;
        }

        private static string? GetValue(XmlNode node, string xpath)
        {
            var resultNode = node.SelectSingleNode(xpath);
            if (resultNode == null)
                return null;
            return resultNode.InnerText;
        }
    }
}
