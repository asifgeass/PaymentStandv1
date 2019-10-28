using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlStructureComplat
{
    [XmlRoot]
    public class Lookup
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlElement]
        public List<LookupItem> Item { get; set; }
    }
}
