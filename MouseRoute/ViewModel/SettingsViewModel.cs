using System.Collections.Generic;
using System.Globalization;
using GalaSoft.MvvmLight;
using MouseRoute.Model;

namespace MouseRoute.ViewModel {
    public class SettingsViewModel : ViewModelBase {
        private MouseSettings _settings;

        public SettingsViewModel() {
            _settings = MouseSettings.ReadSettings() ?? new MouseSettings();
        }

        #region Properties

        public bool Autostart {
            get { return _settings.AutoStart; }
            set {
                _settings.AutoStart = value;
                RaisePropertyChanged("StartOnStartup");
            }
        }

        public bool Autorun {
            get { return _settings.AutoRun; }
            set {
                _settings.AutoRun = value;
                RaisePropertyChanged("RunWithWindows");
            }
        }

        public CultureInfo SelectedLanguage {
            get { return _settings.SelectedLanguage; }
            set {
                _settings.SelectedLanguage = value;
                RaisePropertyChanged("SelectedLanguage");
            }
        }

        public void Save() {
            MouseSettings.Save(_settings);
        }
        public override void Cleanup() {
            Save();
            base.Cleanup();
        }
        public List<CultureInfo> Languages { get { return _settings.Languages; } }
        #endregion
    }
}
