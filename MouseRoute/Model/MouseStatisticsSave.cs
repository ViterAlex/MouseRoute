using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MouseRoute.Model {
    /// <summary>
    /// Класс для сериализации статистических данных об использовании мыши
    /// </summary>
    [XmlRoot(ElementName = "Statistics")]
    public class MouseStatisticsSave  {
        [XmlAttribute]
        public int TotalLeftClicks { get; set; }
        [XmlAttribute]
        public int TotalRightClicks { get; set; }
        [XmlAttribute]
        public int TotalMiddleClicks { get; set; }
        [XmlAttribute]
        public double TotalRoute { get; set; }
        public List<MouseData> MouseDataList { get; set; }
        public MouseStatisticsSave() {

        }
        public MouseStatisticsSave(MouseStatistics statistics) {
            MouseDataList = new List<MouseData>();
            foreach (var item in statistics) {
                MouseDataList.Add(item);
            }
        }
        private const string STATISTICS_FILENAME = "statistics.xml";

        public static void Save(MouseStatisticsSave statistic) {
            XmlSerializer deser = new XmlSerializer(statistic.GetType());
            TextWriter tw = new StreamWriter(STATISTICS_FILENAME);
            deser.Serialize(tw, statistic);
        }

        public static MouseStatisticsSave Read() {
            if (!File.Exists(STATISTICS_FILENAME)) {
                return null;
            }
            XmlSerializer deser = new XmlSerializer(typeof(MouseStatisticsSave));
            TextReader tw = new StreamReader(STATISTICS_FILENAME);
            return (MouseStatisticsSave)deser.Deserialize(tw);
        }
    }
}
