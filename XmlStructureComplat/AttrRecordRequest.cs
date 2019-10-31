using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlStructureComplat
{
    [XmlRoot]
    public class AttrRecordRequest
    {
        [XmlAttribute()]
        public string Code { get; set; }
        [XmlAttribute()]
        public string Name { get; set; }
        [XmlAttribute()]
        public string Value { get; set; } = string.Empty;
        [XmlAttribute()]
        public string CodeOut { get; set; }
        [XmlAttribute()]
        public short Change { get; set; }
        [XmlAttribute()]
        public string Level { get; set; }
        public AttrRecordRequest Accept(AttrRecord arg)
        {
            Name = arg?.Name ?? Name;
            Code = arg?.Code ?? Code;
            Value = arg?.Value ?? Value;
            CodeOut = arg?.CodeOut ?? CodeOut;
            return this;
        }
        public AttrRecordRequest(AttrRecord arg) : this()
        {
            this.Accept(arg);
        }
        public AttrRecordRequest() { }
    }
}
