using System;
using MouseRoute.Model;
namespace MouseRoute.Design {
    /// <summary>
    /// Отображение данных мыши в режиме разработки
    /// </summary>
    public class DesignMouseController : MouseController {
        public DesignMouseController() {
            Random rnd = new Random();
            LeftClicks = rnd.Next(100);
            RightClicks = rnd.Next(100);
            MiddleClicks = rnd.Next(100);
            for (int i = 0; i < 10; i++) {
                AddRoute(rnd.Next(100), rnd.Next(100));
            }
        }
    }
}
