using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight.Ioc;
namespace MouseRoute.Model {
    public class MouseStatistics : ObservableCollection<MouseData> {

        private MouseData _current;

        public MouseStatistics(MouseController mouse) {
            mouse.Started += (s, e) => {
                Add(new MouseData() {
                    LeftClicks = mouse.LeftClicks,
                    RightClicks = mouse.RightClicks,
                    MiddleClicks = mouse.MiddleClicks,
                    Route = mouse.Route
                });
                _current = Items.Last();
                _current.Start();
            };
            mouse.Stopped += (s, e) => {
                _current.Stop();
                _current = null;
            };
            mouse.PropertyChanged += (s, e) => {
                if (_current == null) {
                    return;
                }
                _current.LeftClicks = mouse.LeftClicks;
                _current.RightClicks = mouse.RightClicks;
                _current.MiddleClicks = mouse.MiddleClicks;
                _current.Route = mouse.Route;
            };
        }
    }
}
