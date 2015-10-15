using System;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
namespace MouseRoute.Model {
    /// <summary>
    /// Данные об использовании мыши за определённый промежуток времени
    /// </summary>
    public class MouseData : ObservableObject {
        /// <summary>
        /// Время начала сеанса
        /// </summary>
        public DateTime StartTime { get; set; }
        private DateTime endTime;

        /// <summary>
        /// Длительность сеанса слежения за мышью
        /// </summary>
        public TimeSpan Duration {
            get {
                return endTime == StartTime ? DateTime.Now - StartTime : endTime - StartTime;
            }
            set {
                endTime = StartTime + value;
            }
        }
        private int leftClicks;
        /// <summary>
        /// Количество кликов левой кнопкой в данном сеансе
        /// </summary>
        public int LeftClicks {
            get { return leftClicks; }
            set {
                Set("LeftClicks", ref leftClicks, value);
            }
        }
        private int rightClicks;
        /// <summary>
        /// Количество кликов правой кнопкой в данном сеансе
        /// </summary>
        public int RightClicks {
            get { return rightClicks; }
            set {
                Set("RightClicks", ref rightClicks, value);
            }
        }
        private int middleClicks;
        /// <summary>
        /// Количество кликов средней кнопкой в данном сеансе
        /// </summary>
        public int MiddleClicks {
            get { return middleClicks; }
            set {
                Set("MiddleClicks", ref middleClicks, value);
            }
        }
        private double route;
        /// <summary>
        /// Расстояние пройденное мышью по экрану
        /// </summary>
        public double Route {
            get { return route; }
            set {
                Set("Route", ref route, value);
            }
        }

        private DispatcherTimer timer;

        public void Stop() {
            timer.Stop();
        }
        public void Start() {
            StartTime = DateTime.Now;
            timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(100) };
            timer.Tick += (s, e) => {
                endTime = DateTime.Now;
                RaisePropertyChanged("Duration");
            };
            timer.Start();
        }
    }
}
