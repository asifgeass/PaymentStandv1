using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlStructureComplat;
using XmlStructureComplat.MDOM_POS;

namespace Logic
{
    public class QAMdomPOS
    {
        #region fields
        private static string url = @"http://public.softclub.by:8010/mdom_pos/online.request";
        private string TerminalId = "KKM1";
        private string Version = "1";
        private string PC_ID = "000000000000";
        #endregion
        #region Prop
        public MDOM_POS Request { get; set; }
        public MDOM_POS Response { get; set; }
        public static string Url => url;
        #endregion
        #region ctor
        private QAMdomPOS()
        {
            Request = new MDOM_POS();
            Request.Root2.TerminalId = TerminalId;
            Request.Root2.Version = Version;
            Request.Root2.PC_ID = PC_ID;
        }
        public QAMdomPOS(PayRecord payrecArg) : this()
        {
            Request.Root2.PaySumma = payrecArg.Summa;
        }
        #endregion
    }
}
