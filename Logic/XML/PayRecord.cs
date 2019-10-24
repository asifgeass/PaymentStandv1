using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logic.XML
{
    [XmlRoot]
    public class PayRecord
    {
        [XmlElement()]
        public GroupRecord GroupRecord { get; set; }
        [XmlArrayItem("Lookup")]
        public List<Lookup> Lookups { get; set; } = null;
        [XmlElement()]
        public List<AttrRecord> AttrRecord { get; set; }
        [XmlAttribute()]
        public string Code { get; set; }
        [XmlAttribute()]
        public string Name { get; set; }
        [XmlAttribute()]
        public string StornoMode { get; set; }
        [XmlAttribute()]
        public string Commission { get; set; }
        [XmlAttribute()]
        public string Summa { get; set; }
        [XmlAttribute()]
        public short View { get; set; } = 1;
        [XmlAttribute()]
        public string Fine { get; set; }
        [XmlAttribute()]
        public string PayCommission { get; set; }
        [XmlAttribute()]
        public string SessionId { get; set; }
        [XmlAttribute()]
        public string Edit { get; set; }
        [XmlAttribute()]
        public string GetPayListType { get; set; }
        [XmlAttribute()]
        public string RecordID { get; set; }
        [XmlAttribute()]
        public string PaymentID { get; set; }
        public string xx { get; set; }
    }
}
