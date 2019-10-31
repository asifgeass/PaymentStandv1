using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlStructureComplat
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
        [XmlElement("UnknownResponse")]
        [XmlElement("ConfirmResponse")]
        public GetPayResponse GetListResponse { get; set; }
    }

    [XmlType(IncludeInSchema = false)]
    public enum ItemChoiceType
    {
        GetPayListResponse,
        GetPayListRequest,
        RunOperationResponse,
        UnknownResponse,
        ConfirmResponse
    }
}
