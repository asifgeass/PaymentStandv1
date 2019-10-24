using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logic.XML
{
    [XmlRoot]
    public partial class PS_ERIP
    {
        [XmlIgnore]
        public ItemChoiceType EnumType;

        [XmlChoiceIdentifier("EnumType")]
        [XmlElement("GetPayListResponse")]
        [XmlElement("RunOperationResponse")]
        [XmlElement("ConfirmResponse")]
        public GetPayListResponse GetListResponse { get; set; }
    }

    [XmlType(IncludeInSchema = false)]
    public enum ItemChoiceType
    {
        GetPayListResponse,
        RunOperationResponse,
        ConfirmResponse
    }
}
