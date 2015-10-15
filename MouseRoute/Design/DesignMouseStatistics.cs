using System;
using System.Linq;
using MouseRoute.Model;
using System.Threading;
namespace MouseRoute.Design {
    internal class DesignMouseStatistics : MouseStatistics {
        public DesignMouseStatistics()
            : base(new MouseController()) {
            Random rnd = new Random();
            for (int i = 0; i < 3; i++) {
                Add(new MouseData() {
                    LeftClicks = rnd.Next(20),
                    RightClicks = rnd.Next(20),
                    MiddleClicks = rnd.Next(20),
                    Route = rnd.Next(1000) / 2.3d,
                });
                Items.Last().Start();
                Thread.Sleep(rnd.Next(300));
                Items.Last().Stop();
            }
        }
    }
}