using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
namespace MouseRoute.Model {
    public class MouseSettings {
        #region Properties
        private bool _autoStart;
        /// <summary>
        /// Начать слежение при старте программы
        /// </summary>
        public bool AutoStart {
            get { return _autoStart; }
            set { _autoStart = value; }
        }
        private bool _autoRun;
        /// <summary>
        /// Автозапуск приложения вместе с Windows
        /// </summary>
        public bool AutoRun {
            get { return _autoRun; }
            set {
                _autoRun = value;
                SetAutorunWithWindows(_autoRun);
            }
        }
        private CultureInfo _selectedLanguage;
        /// <summary>
        /// Выбранный язык приложения
        /// </summary>
        [XmlIgnore]
        public CultureInfo SelectedLanguage {
            get { return _selectedLanguage; }
            set {
                _selectedLanguage = value;
                App.Language = _selectedLanguage;
                _selectedLanguageName = App.Language.Name;
            }
        }
        private string _selectedLanguageName;

        /// <summary>
        /// Имя выбранного языка
        /// </summary>
        public string SelectedLanguageName {
            get { return _selectedLanguageName; }
            set { _selectedLanguageName = value; }
        }

        [XmlIgnore]
        public List<CultureInfo> Languages { get; set; }

        #endregion

        #region Constants
        private const string REGISTRY_APP_NAME = "MouseRoute";
        private const string SETTINGS_FILENAME = @"settings.xml";
        #endregion

        public MouseSettings() {

        }
        /// <summary>
        /// Чтение файла с настройками
        /// </summary>
        static public MouseSettings ReadSettings() {
            MouseSettings ms = null;
            if (File.Exists(SETTINGS_FILENAME)) {
                using (TextReader tr = new StreamReader(SETTINGS_FILENAME)) {
                    XmlSerializer xs = new XmlSerializer(typeof(MouseSettings));
                    ms = (MouseSettings)xs.Deserialize(tr);
                    ms.CheckRegistry();
                    if (ms.AutoStart) {
                        Messenger.Default.Send(new NotificationMessage<MouseSettings>(ms, string.Empty));
                    }
                }
            }
            else ms = new MouseSettings();
            ms.Languages = new List<CultureInfo>();
            ReadLanguages(ms);
            if (ms.SelectedLanguageName == null) {
                ms.SelectedLanguageName = "en-US";
            }
            ms.SelectedLanguage = ms.Languages.Find(l => l.Name == ms.SelectedLanguageName);
            return ms;
        }
        /// <summary>
        /// Чтение файлов с языками
        /// </summary>
        private static void ReadLanguages(MouseSettings ms) {
            string cult;
            foreach (var item in Directory.GetFiles("Languages", "lang.*.xaml")) {
                cult = item.Substring(item.IndexOf('.') + 1);
                cult = cult.Substring(0, cult.LastIndexOf('.'));
                ms.Languages.Add(new CultureInfo(cult));
            }
        }

        /// <summary>
        /// Проверка параметров автозапуска в реестре
        /// </summary>
        private void CheckRegistry() {
            using (RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)) {
                /// Если запускать вместе с Windows
                /// то в реестре должна быть запись.
                if (_autoRun) {
                    if (rkApp.GetValue(REGISTRY_APP_NAME) == null) {
                        rkApp.SetValue(REGISTRY_APP_NAME, Assembly.GetExecutingAssembly().Location.ToString(), RegistryValueKind.String);
                    }
                }
                /// Если не запускать вместе с Windows
                /// то в реестре не должно быть записи.
                else {
                    if (rkApp.GetValue(REGISTRY_APP_NAME) != null) {
                        rkApp.DeleteValue(REGISTRY_APP_NAME);
                    }
                }
            }
        }

        public static void Save(MouseSettings ms) {
            using (TextWriter tw = new StreamWriter(SETTINGS_FILENAME)) {
                XmlSerializer xs = new XmlSerializer(ms.GetType());
                xs.Serialize(tw, ms);
            }
        }

        private void SetAutorunWithWindows(bool value) {

            using (RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)) {
                if (rkApp.GetValue(REGISTRY_APP_NAME) != null) {
                    if (!value) {
                        rkApp.DeleteValue(REGISTRY_APP_NAME);
                    }
                }
                else {
                    if (value) {
                        rkApp.SetValue(REGISTRY_APP_NAME, Assembly.GetExecutingAssembly().Location.ToString(), RegistryValueKind.String);
                    }
                }
            }
        }
    }
}
