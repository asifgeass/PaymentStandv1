using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logic.XML.CheckXml
{
    public partial class CheckLine
    {
        [XmlAttribute]
        public int Idx { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}
