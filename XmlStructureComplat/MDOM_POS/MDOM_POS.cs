using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlStructureComplat
{
    [XmlRoot]
    public class MDOM_POS
    {
        [XmlIgnore]
        public PosQAType EnumType;

        [XmlChoiceIdentifier("EnumType")]
        [XmlElement("UnknownResponse")]
        [XmlElement("PURRequest")]
        [XmlElement("PURResponse")]
        [XmlElement("VOIRequest")]
        [XmlElement("VOIResponse")]
        [XmlElement("CFGRequest")]
        public Root2QA ResponseReq { get; set; } = new Root2QA();
    }
    [XmlType(IncludeInSchema = false)]
    public enum PosQAType
    {
        UnknownResponse,
        PURRequest,
        PURResponse,
        VOIRequest,
        VOIResponse,
        CFGRequest
    }
}
