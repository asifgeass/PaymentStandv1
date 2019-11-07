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
            Request.ResponseReq.TerminalId = TerminalId;
            Request.ResponseReq.Version = Version;
            Request.ResponseReq.PC_ID = PC_ID;
            Request.EnumType = PosQAType.PURRequest;
        }
        public QAMdomPOS(PayRecord payrecArg) : this()
        {
            Request.EnumType = PosQAType.PURRequest;
            Request.ResponseReq.PaySumma = payrecArg.Summa;
            //TEST
            if (Request.ResponseReq.PaySumma == "0.00")
            {
                Request.ResponseReq.PaySumma = "1";
            }
            //TEST
        }
        #endregion
    }
}
