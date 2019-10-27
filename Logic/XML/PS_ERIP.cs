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
        [XmlElement("GetPayListRequest")]
        [XmlElement("RunOperationResponse")]
        [XmlElement("ConfirmResponse")]
        public GetListResponse GetListResponse { get; set; }
    }

    [XmlType(IncludeInSchema = false)]
    public enum ItemChoiceType
    {
        GetPayListResponse,
        GetPayListRequest,
        RunOperationResponse,
        ConfirmResponse
    }
}
