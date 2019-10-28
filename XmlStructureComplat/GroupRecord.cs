using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlStructureComplat
{
    [XmlRoot]
    public class GroupRecord
    {
        [XmlAttribute()]
        public string Code { get; set; }
        [XmlAttribute()]
        public string Name { get; set; }
    }
}
