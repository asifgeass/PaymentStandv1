using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logic.XML
{
    [XmlRoot]
    public partial class GetListResponse
    {
        private int errorCodeField;

        private List<PayRecord> payRecordField;

        /// <remarks/>
        public int ErrorCode
        {
            get
            {
                return this.errorCodeField;
            }
            set
            {
                this.errorCodeField = value;
            }
        }
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
        public List<PayRecord> PayRecord
        {
            get
            {
                return this.payRecordField;
            }
            set
            {
                this.payRecordField = value;
            }
        }
    }
}
