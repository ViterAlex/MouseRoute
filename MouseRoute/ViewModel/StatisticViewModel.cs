using GalaSoft.MvvmLight;
using MouseRoute.Model;
using GalaSoft.MvvmLight.Messaging;
namespace MouseRoute.ViewModel {
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class StatisticViewModel : ViewModelBase {
        private MouseStatistics _statistics;
        public MouseStatistics Statistics { get { return _statistics; } }
        /// <summary>
        /// Initializes a new instance of the StatisticViewModel class.
        /// </summary>
        public StatisticViewModel(MouseStatistics statistics) {
            this._statistics = statistics;
        }
    }
}