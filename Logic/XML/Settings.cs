using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Logic.XML;

namespace Logic
{
    [XmlRoot]
    public class Settings
    {
        public EripComplatSettings ERIP { get; set; } = new EripComplatSettings();
        public TerminalSettings Terminal_MdomPOS { get; set; } = new TerminalSettings();
    }
}
