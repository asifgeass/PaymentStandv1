using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlStructureComplat
{
    [XmlRoot]
    public class AttrRecord
    {
        [XmlAttribute()]
        public float Code { get; set; }
        [XmlAttribute()]
        public string Name { get; set; }
        [XmlAttribute()]
        public string Value { get; set; } = string.Empty;
        [XmlAttribute()]
        public short View { get; set; } = 1;
        [XmlAttribute()]
        public short Edit { get; set; }
        [XmlAttribute()]
        public short Change { get; set; }
        [XmlAttribute()]
        public float CodeOut { get; set; }
        [XmlAttribute()]
        public string Type { get; set; }
        [XmlAttribute()]
        public string Mandatory { get; set; }
        [XmlAttribute()]
        public string MinLength { get; set; }
        [XmlAttribute()]
        public string MaxLength { get; set; }
        [XmlAttribute()]
        public string Min { get; set; }
        [XmlAttribute()]
        public string Max { get; set; }
        [XmlAttribute()]
        public string Lookup { get; set; }
        [XmlAttribute()]
        public string Hint { get; set; }
        [XmlAttribute()]
        public string Print { get; set; }
        [XmlAttribute()]
        public string Format { get; set; }
    }
}
