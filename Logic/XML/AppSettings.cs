using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logic.XML
{
    [XmlRoot]
    public class AppSettings
    {
        public string DisablePrintCheck { get; set; } = "0";
    }
}
