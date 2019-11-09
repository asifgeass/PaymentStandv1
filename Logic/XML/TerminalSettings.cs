using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logic.XML
{
    [XmlRoot]
    public class TerminalSettings
    {
        public string url { get; set; } = @"http://public.softclub.by:8010/mdom_pos/online.request";
        public string TerminalID { get; set; } = "KKM1";
        public string version { get; set; } = "1";
    }
}
