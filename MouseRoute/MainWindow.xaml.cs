using System.Windows;
using MouseRoute.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System;

namespace MouseRoute {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow() {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
            Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);
        }

        private void NotificationMessageReceived(NotificationMessage msg) {
            switch (msg.Notification) {
                case "ShowStatistic":
                    //(new StatisticsView()).Show();
                    break;
                default:
                    break;
            }
        }
    }
}