﻿/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:MouseRoute.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using MouseRoute.Model;

namespace MouseRoute.ViewModel {
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator {
        static ViewModelLocator() {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic) {
                SimpleIoc.Default.Register<MouseSettings, Design.DesignMouseSettings>();
                SimpleIoc.Default.Register<MouseStatistics, Design.DesignMouseStatistics>();
                SimpleIoc.Default.Register<MouseController, Design.DesignMouseController>();
            }
            else {
                SimpleIoc.Default.Register<MouseSettings, MouseSettings>();
                SimpleIoc.Default.Register<MouseStatistics, MouseStatistics>();
                SimpleIoc.Default.Register<MouseController, MouseController>();
            }

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<StatisticViewModel>();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main {
            get {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        /// <summary>
        /// Gets the Settings property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public SettingsViewModel Settings {
            get {
                return ServiceLocator.Current.GetInstance<SettingsViewModel>();
            }
        }
        /// <summary>
        /// Gets the Statistics property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public StatisticViewModel Statistics {
            get {
                return ServiceLocator.Current.GetInstance<StatisticViewModel>();
            }
        }/// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup() {
            
        }
    }
}