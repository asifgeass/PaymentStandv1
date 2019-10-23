using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logic.XML
{
    [XmlRoot]
    public partial class GetPayListResponse
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
