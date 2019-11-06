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
        public GetPayResponse RootQAType { get; set; } = new GetPayResponse();
        public PS_ERIP Clear()
        {
            try
            {
                this.RootQAType.PayRecord[0].AttrRecord.ForEach(attr =>
                {
                    attr.MinLength = null;
                    attr.MaxLength = null;
                    attr.Min = null;
                    attr.Max = null;
                    attr.Mandatory = null;
                    attr.Lookup = null;
                    attr.Type = null;
                    attr.Hint = null;
                });
            }
            catch (Exception) { }
            return this;
        }
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
        ConfirmResponse
    }
}
