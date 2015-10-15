using System.Windows;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Threading;
using System.Globalization;
using System.Linq;

namespace MouseRoute {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        static App() {
            DispatcherHelper.Initialize();
        }
        /// <summary>
        /// Текущий язык приложения
        /// </summary>
        public static CultureInfo Language {
            get { return Thread.CurrentThread.CurrentUICulture; }
            set {
                if (value == Thread.CurrentThread.CurrentUICulture) {
                    return;
                }
                //Установка нового языка
                Thread.CurrentThread.CurrentUICulture = value;
                //Подгрузка нового ресурса
                ResourceDictionary dict = new ResourceDictionary();
                Uri uri = new Uri(string.Format("Languages/lang.{0}.xaml", value.Name), UriKind.Relative);
                if (System.IO.File.Exists(uri.OriginalString)) {
                    dict.Source = uri;
                }
                //Удаление старого ресурса
                ResourceDictionary oldDict = Current.Resources.MergedDictionaries.Where(d => d.Source != null && d.Source.OriginalString.StartsWith("Languages/lang")).First();
                if (oldDict != null) {
                    int index = Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Current.Resources.MergedDictionaries.RemoveAt(index);
                    Current.Resources.MergedDictionaries.Insert(index, dict);
                }
                else
                    Current.Resources.MergedDictionaries.Add(dict);
            }
        }

    }
}
