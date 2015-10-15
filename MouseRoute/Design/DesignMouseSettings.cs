using System.Collections.Generic;
using System.Globalization;

namespace MouseRoute.Design {
    internal class DesignMouseSettings : Model.MouseSettings {
        public DesignMouseSettings() {
            AutoRun = true;
            AutoStart = true;
            Languages = new List<CultureInfo>();
            Languages.AddRange(new CultureInfo[] {
                new CultureInfo("ru-RU"),
                new CultureInfo("en-US")
            });
        }
    }
}