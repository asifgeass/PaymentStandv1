using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logic.XML
{
    //[System.SerializableAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [XmlRoot]
    public partial class GetPayListResponse
    {
        private short errorCodeField;

        private List<PayRecord> payRecordField;

        /// <remarks/>
        public short ErrorCode
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
