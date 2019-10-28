using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlStructureComplat.CheckFolder
{
    public partial class CheckLine
    {
        [XmlAttribute]
        public int Idx { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}
