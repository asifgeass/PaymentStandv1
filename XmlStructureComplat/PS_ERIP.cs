using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlStructureComplat
{
    [XmlRoot]
    public class PS_ERIP
    {
        #region properties
        [XmlIgnore]
        public EripQAType EnumType;

        [XmlChoiceIdentifier("EnumType")]
        [XmlElement("GetPayListResponse")]
        [XmlElement("GetPayListRequest")]
        [XmlElement("RunOperationRequest")]
        [XmlElement("ConfirmRequest")]
        [XmlElement("RunOperationResponse")]
        [XmlElement("UnknownResponse")]
        [XmlElement("ConfirmResponse")]
        [XmlElement("POSPayResponse")]
        [XmlElement("POSCancelResponse")]
        public GetPayResponse ResponseReq { get; set; } = new GetPayResponse();
        #endregion
    }

    [XmlType(IncludeInSchema = false)]
    public enum EripQAType
    {
        GetPayListResponse,
        GetPayListRequest,
        RunOperationRequest,
        ConfirmRequest,
        RunOperationResponse,
        UnknownResponse,
        POSPayResponse,
        POSCancelResponse,
        ConfirmResponse
    }
}
