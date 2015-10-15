using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MouseRoute.Model;

namespace MouseRoute.ViewModel {
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase {
        #region Fields & Properties
        private readonly MouseController _mouseController;
        private readonly MouseStatistics _statistics;
        private int _oldLeftClicks;
        private int _oldRightClicks;
        private int _oldMiddleClicks;
        private double _oldRoute;

        public int TotalLeftClicks { get { return MouseController.TotalLC + _oldLeftClicks; } }
        public int TotalRightClicks { get { return MouseController.TotalRC + _oldRightClicks; } }
        public int TotalMiddleClicks { get { return MouseController.TotalMC + _oldMiddleClicks; } }
        public double TotalRoute { get { return MouseController.TotalRoute + _oldRoute; } }
        public bool IsHooked { get { return _mouseController.IsHooked; } }
        #endregion

        #region Commands
        public RelayCommand OnOffCommand { get; private set; }
        public RelayCommand ShowStaticticsCommand { get; private set; }
        #endregion

        public MainViewModel(MouseController mouse, MouseStatistics statistics) {
            _mouseController = mouse;
            _statistics = statistics;
            ReadSavedData();

            mouse.PropertyChanged += (s, e) => RaisePropertyChanged(e.PropertyName);

            OnOffCommand = new RelayCommand(OnOff);
            ShowStaticticsCommand = new RelayCommand(ShowStatistics);

            //Подписка на сообщение для получения настроек
            Messenger.Default.Register<NotificationMessage<MouseSettings>>(this, (e) => {
                if (e.Content.AutoStart) {
                    OnOff();
                }
            });
        }

        private void ShowStatistics() {
            Messenger.Default.Send(new NotificationMessage("ShowStatistic"));
        }

        private void OnOff() {
            if (_mouseController.IsHooked) {
                _mouseController.Stop();
            }
            else {
                _mouseController.Start();
            }
        }

        ~MainViewModel() {
            Cleanup();
        }

        /// <summary>
        /// Чтение сохранённых данных
        /// </summary>
        private void ReadSavedData() {
            MouseStatisticsSave mss = MouseStatisticsSave.Read();
            if (mss == null) {
                return;
            }
            _oldLeftClicks = mss.TotalLeftClicks;
            _oldRightClicks = mss.TotalRightClicks;
            _oldMiddleClicks = mss.TotalMiddleClicks;
            _oldRoute = mss.TotalRoute;
            foreach (var item in mss.MouseDataList) {
                _statistics.Add(item);
            }
        }

        public override void Cleanup() {
            MouseStatisticsSave.Save(new MouseStatisticsSave(_statistics) {
                TotalLeftClicks = TotalLeftClicks,
                TotalRightClicks = TotalRightClicks,
                TotalMiddleClicks = TotalMiddleClicks,
                TotalRoute = TotalRoute
            });
            base.Cleanup();
        }
    }
}