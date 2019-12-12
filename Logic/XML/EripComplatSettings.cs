using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logic.XML
{
    [XmlRoot]
    public class EripComplatSettings
    {
        public string url { get; set; } = @"http://public.softclub.by:3007/komplat/online.request";
        public string TerminalID { get; set; } = "TEST_TERMINAL";
        public string version { get; set; } = "3";
        public string HomePaycode { get; set; } = "400";
    }
}
