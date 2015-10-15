using GalaSoft.MvvmLight;
using System;
using System.ComponentModel;
using System.Windows;
namespace MouseRoute.Model {
    public class MouseController : ObservableObject {

        #region Свойства и поля
        private int _leftClicks;
        public int LeftClicks {
            get { return _leftClicks; }
            set {
                int oldValue = _leftClicks;
                Set("LeftClicks", ref _leftClicks, value);
                Set("TotalLeftClicks", ref _totalLC, _totalLC + value - oldValue);
            }
        }
        private int _rightClicks;
        public int RightClicks {
            get { return _rightClicks; }
            set {
                int oldValue = _rightClicks;
                Set("RightClicks", ref _rightClicks, value);
                Set("TotalRightClicks", ref _totalRC, _totalRC + value - oldValue);
            }
        }
        private int _middleClicks;
        public int MiddleClicks {
            get { return _middleClicks; }
            set {
                int oldValue = _middleClicks;
                Set("MiddleClicks", ref _middleClicks, value);
                Set("TotalMiddleClicks", ref _totalMC, _totalMC + value - oldValue);
            }
        }

        private double _route;
        public double Route {
            get { return _route; }
        }

        public bool IsHooked {
            get { return _mh.IsHooked; }
        }

        internal void Reset() {
            LeftClicks = 0;
            RightClicks = 0;
            MiddleClicks = 0;
            _firstCall = true;
        }

        private Vector _lastVector;
        private bool _firstCall = true;
        private RamGecTools.MouseHook _mh;
        #endregion
        public MouseController() {
            _mh = new RamGecTools.MouseHook();
            _mh.LeftButtonUp += (mouseStruct) => {
                LeftClicks++;
            };
            _mh.RightButtonUp += (mouseStruct) => {
                RightClicks++;
            };
            _mh.MiddleButtonUp += (mouseStruct) => {
                MiddleClicks++;
            };
            _mh.DoubleClick += (mouseStruct) => {
                //DoubleLeftClicks++;
            };
            _mh.MouseMove += (mouseStruct) => {
                AddRoute(mouseStruct.pt.x, mouseStruct.pt.y);
            };
        }
        internal void Start() {
            _mh.Install();
            RaisePropertyChanged("IsHooked");
            OnMouseStarted();
        }

        internal void Stop() {
            _mh.Uninstall();
            RaisePropertyChanged("IsHooked");
            OnMouseStopped();
        }
        #region Static properties & methods
        static private int _totalLC;

        static public int TotalLC {
            get { return _totalLC; }
        }
        static private int _totalRC;

        static public int TotalRC {
            get { return _totalRC; }
        }
        static private int _totalMC;

        static public int TotalMC {
            get { return _totalMC; }
        }
        static private double _totalRoute;

        static public double TotalRoute {
            get { return _totalRoute; }
        }

        //public bool IsHooked { get { return } }

        #endregion
        /// <summary>
        /// Вычисление пройденного пути
        /// </summary>
        public void AddRoute(int x, int y) {
            Vector v = new Vector(x, y);

            if (_firstCall) {
                Set("Route", ref _route, 0);
                _firstCall = false;
            }
            else {
                Set("TotalRoute", ref _totalRoute, _totalRoute + Vector.Subtract(v, _lastVector).Length);
                Set("Route", ref _route, _route + Vector.Subtract(v, _lastVector).Length);
            }
            _lastVector = v;
        }
        public event EventHandler Started;
        protected void OnMouseStarted() {
            EventHandler temp = Started;
            if (temp != null) {
                temp(this, EventArgs.Empty);
            }
        }
        public event EventHandler Stopped;
        protected void OnMouseStopped() {
            EventHandler temp = Stopped;
            if (temp != null) {
                temp(this, EventArgs.Empty);
            }
        }

    }
}
