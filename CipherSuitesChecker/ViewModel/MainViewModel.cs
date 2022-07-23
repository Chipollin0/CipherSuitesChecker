using CipherSuitesChecker.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace CipherSuitesChecker.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<CipherSuite> cipherSuites;
        private ObservableCollection<CipherSuite> filteredCipherSuites;
        private CipherSuite selectedCipherSuite;
        private string filterName;
        private bool filterTls1;
        private bool filterTls11;
        private bool filterTls12;
        private bool filterTls13;
        private bool filterSecure;
        private bool filterRecommended;
        private bool filterWeak;
        private bool filterInsecure;
        private string filterComment;
        private bool isEnabled;

        #region Constructors

        public MainViewModel()
        {
            filterName = "";
            filterTls1 = true;
            filterTls11 = true;
            filterTls12 = true;
            filterTls13 = true;
            filterSecure = true;
            filterRecommended = true;
            filterWeak = true;
            filterInsecure = true;
            filterComment = "";
            cipherSuites = new ObservableCollection<CipherSuite>();
            filteredCipherSuites = new ObservableCollection<CipherSuite>();
            selectedCipherSuite = new CipherSuite();
        }

        #endregion

        #region Properties

        public ObservableCollection<CipherSuite> CipherSuites
        {
            get { return cipherSuites; }
            set
            {
                if (cipherSuites == value)
                    return;
                cipherSuites = value;
                OnPropertyChanged(nameof(CipherSuites));
                UpdateFilteredCipherSuites();
            }
        }

        public ObservableCollection<CipherSuite> FilteredCipherSuites
        {
            get { return filteredCipherSuites; }
            set
            {
                if (filteredCipherSuites == value)
                    return;
                filteredCipherSuites = value;
                OnPropertyChanged(nameof(FilteredCipherSuites));
                UpdateSelectedCipherSuite();
            }
        }

        public CipherSuite SelectedCipherSuite
        {
            get { return selectedCipherSuite; }
            set
            {
                if (selectedCipherSuite == value)
                    return;
                selectedCipherSuite = value;
                OnPropertyChanged(nameof(SelectedCipherSuite));
            }
        }

        public string FilterName
        {
            get { return filterName; }
            set
            {
                if (filterName == value)
                    return;
                filterName = value;
                OnPropertyChanged(nameof(FilterName));
                UpdateFilteredCipherSuites();
            }
        }

        public bool FilterTls1
        {
            get { return filterTls1; }
            set
            {
                if (filterTls1 == value)
                    return;
                filterTls1 = value;
                OnPropertyChanged(nameof(FilterTls1));
                UpdateFilteredCipherSuites();
            }
        }

        public bool FilterTls11
        {
            get { return filterTls11; }
            set
            {
                if (filterTls11 == value)
                    return;
                filterTls11 = value;
                OnPropertyChanged(nameof(FilterTls11));
                UpdateFilteredCipherSuites();
            }
        }

        public bool FilterTls12
        {
            get { return filterTls12; }
            set
            {
                if (filterTls12 == value)
                    return;
                filterTls12 = value;
                OnPropertyChanged(nameof(FilterTls12));
                UpdateFilteredCipherSuites();
            }
        }

        public bool FilterTls13
        {
            get { return filterTls13; }
            set
            {
                if (filterTls13 == value)
                    return;
                filterTls13 = value;
                OnPropertyChanged(nameof(FilterTls13));
                UpdateFilteredCipherSuites();
            }
        }

        public bool FilterSecure
        {
            get { return filterSecure; }
            set
            {
                if (filterSecure == value)
                    return;
                filterSecure = value;
                OnPropertyChanged(nameof(FilterSecure));
                UpdateFilteredCipherSuites();
            }
        }

        public bool FilterRecommended
        {
            get { return filterRecommended; }
            set
            {
                if (filterRecommended == value)
                    return;
                filterRecommended = value;
                OnPropertyChanged(nameof(FilterRecommended));
                UpdateFilteredCipherSuites();
            }
        }

        public bool FilterWeak
        {
            get { return filterWeak; }
            set
            {
                if (filterWeak == value)
                    return;
                filterWeak = value;
                OnPropertyChanged(nameof(FilterWeak));
                UpdateFilteredCipherSuites();
            }
        }

        public bool FilterInsecure
        {
            get { return filterInsecure; }
            set
            {
                if (filterInsecure == value)
                    return;
                filterInsecure = value;
                OnPropertyChanged(nameof(FilterInsecure));
                UpdateFilteredCipherSuites();
            }
        }

        public string FilterComment
        {
            get { return filterComment; }
            set
            {
                if (filterComment == value)
                    return;
                filterComment = value;
                OnPropertyChanged(nameof(FilterComment));
                UpdateFilteredCipherSuites();
            }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (isEnabled == value)
                    return;
                isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        #endregion

        #region Actions

        public void Load()
        {
            var configFilePath = GetConfigFilePath();
            if (File.Exists(configFilePath))
            {
                var config = new CipherSuiteConfig();
                config.LoadFile(configFilePath);
                CipherSuites = new ObservableCollection<CipherSuite>(config.CipherSuites);
            }
        }

        public void Save()
        {
            var configFilePath = GetConfigFilePath();
            var config = new CipherSuiteConfig();
            config.CipherSuites = CipherSuites;
            config.Save(configFilePath);
        }

        public void Copy()
        {
            var names = filteredCipherSuites.Select(s => s.Name);
            var joinedString = string.Join(Environment.NewLine, names);
            Clipboard.SetText(joinedString);
        }

        public async void Check()
        {
            IsEnabled = false;
            var cipherSuiteRequest = new CipherSuiteWebSite();
            var cipherSuites = await cipherSuiteRequest.RequestCipherSuites();
            CipherSuites = new ObservableCollection<CipherSuite>(cipherSuites);
            Save();
            IsEnabled = true;
        }

        private static string GetConfigFilePath()
        {
            return "CipherSuites.xml";
        }

        #endregion

        #region Other Methods

        private void UpdateFilteredCipherSuites()
        {
            var newFilteredCipherSuites = new List<CipherSuite>();
            foreach (var cipherSuite in cipherSuites)
            {
                if ((filterTls1 && ContainsProtocol(cipherSuite, CipherSuiteProtocols.Tls1) ||
                    filterTls11 && ContainsProtocol(cipherSuite, CipherSuiteProtocols.Tls11) ||
                    filterTls12 && ContainsProtocol(cipherSuite, CipherSuiteProtocols.Tls12) ||
                    filterTls13 && ContainsProtocol(cipherSuite, CipherSuiteProtocols.Tls13)) &&
                    (filterRecommended && EqualsToString(cipherSuite.Security, CipherSuiteSecurity.Recommended) ||
                    filterSecure && EqualsToString(cipherSuite.Security, CipherSuiteSecurity.Secure) ||
                    filterWeak && EqualsToString(cipherSuite.Security, CipherSuiteSecurity.Weak) ||
                    filterInsecure && EqualsToString(cipherSuite.Security, CipherSuiteSecurity.Insecure)) &&
                    (string.IsNullOrWhiteSpace(filterName) || ContainsString(cipherSuite.Name, filterName)) &&
                    (string.IsNullOrWhiteSpace(filterComment) || ContainsString(cipherSuite.Comment, filterComment)))
                    newFilteredCipherSuites.Add(cipherSuite);
            }

            if (!CompareCipherSuites(filteredCipherSuites.ToList(), newFilteredCipherSuites))
                FilteredCipherSuites = new ObservableCollection<CipherSuite>(newFilteredCipherSuites);
        }

        private bool EqualsToString(string source, string target)
        {
            var sourceNormalized = source.Trim().ToLowerInvariant();
            var targetNormalized = target.Trim().ToLowerInvariant();
            if (sourceNormalized == targetNormalized)
                return true;
            return false;
        }

        private bool ContainsString(string source, string part)
        {
            var sourceNormalized = source.Trim().ToLowerInvariant();
            var partNormalized = part.Trim().ToLowerInvariant();
            if (sourceNormalized.Contains(partNormalized))
                return true;
            return false;
        }

        private bool ContainsProtocol(CipherSuite cipherSuite, string protocol)
        {
            foreach (var cipherSuiteProtocol in cipherSuite.Protocols)
                if (EqualsToString(cipherSuiteProtocol, protocol))
                    return true;
            return false;
        }

        private bool CompareCipherSuites(List<CipherSuite> source, List<CipherSuite> target)
        {
            if (source.Count != target.Count)
                return false;
            for (var i = 0; i < source.Count; i++)
            {
                var sourceItem = source[i];
                var targetItem = target[i];
                if (sourceItem.Name != targetItem.Name)
                    return false;
            }

            return true;
        }

        private void UpdateSelectedCipherSuite()
        {
            var included = false;
            if (selectedCipherSuite != null)
                included = filteredCipherSuites.Any(s => s.Name == selectedCipherSuite.Name);
            if (!included)
                SelectedCipherSuite = filteredCipherSuites.Count > 0 ? filteredCipherSuites.First() : new CipherSuite();
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
}
