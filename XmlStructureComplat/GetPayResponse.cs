using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlStructureComplat
{
    [XmlRoot]
    public partial class GetPayResponse
    {
        private List<PayRecord> payRecordField;
        public int ErrorCode { get; set; }
        public string ErrorText { get; set; }
        public string TerminalID { get; set; }
        public string Version { get; set; }
        public string PayCode { get; set; }
        public string PAN { get; set; }
        public string TypePAN { get; set; }
        public string SessionId { get; set; }
        public string CountAttr { get; set; }
        public string SignCode { get; set; }
        public string PayDate { get; set; }
        public string KioskReceipt { get; set; }
        public string KioskError { get; set; }
        public string PayRecordCount { get; set; }
        [XmlElement()]
        public List<PayRecord> PayRecord { get; set; }
        [XmlElement()]
        public List<AttrRecordRequest> AttrRecord { get; set; }
    }
}
