using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlStructureComplat.MDOM_POS
{
    [XmlRoot]
    public class MDOM_POS
    {
        [XmlIgnore]
        public ItemChoiceType EnumType;

        [XmlChoiceIdentifier("EnumType")]
        [XmlElement("UnknownResponse")]
        [XmlElement("PURRequest")]
        [XmlElement("PURResponse")]
        [XmlElement("VOIRequest")]
        [XmlElement("VOIResponse")]
        [XmlElement("CFGRequest")]
        public Root2QA Root2 { get; set; }
    }
    [XmlType(IncludeInSchema = false)]
    public enum ItemChoiceType
    {
        UnknownResponse,
        PURRequest,
        PURResponse,
        VOIRequest,
        VOIResponse,
        CFGRequest
    }
}
