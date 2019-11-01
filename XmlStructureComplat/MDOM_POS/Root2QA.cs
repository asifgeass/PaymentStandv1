using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlStructureComplat.MDOM_POS
{
    [XmlRoot]
    public class Root2QA
    {
        public int ErrorCode { get; set; }
        public string ErrorText { get; set; }
        public string PayDate { get; set; }
        public string KioskReceipt { get; set; }
        public string KioskError { get; set; }
        public string PC_ID { get; set; }
        public string PAN { get; set; }
        public string TypePAN { get; set; }
        public string Receipt { get; set; }
        public string Lang { get; set; }
    }
}
