using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlStructureComplat;

namespace Logic
{
    public class POSManager
    {
        #region fields
        private static string url = @"http://public.softclub.by:8010/mdom_pos/online.request";
        private string TerminalId = "KKM1";
        private string Version = "1";
        #endregion
        #region Prop
        public MDOM_POS Request { get; set; }
        public MDOM_POS Response { get; set; }
        public static string Url => url;
        #endregion
        #region ctor
        public POSManager()
        {
            Request = new MDOM_POS();
            Request.ResponseReq.TerminalId = TerminalId;
            Request.ResponseReq.Version = Version;
            Request.EnumType = PosQAType.VOIRequest;
        }
        public POSManager(string argPCID, string argKioskReceipt):this()
        {
            Request.EnumType = PosQAType.VOIRequest;
            Request.ResponseReq.PC_ID = argPCID;
            Request.ResponseReq.KioskReceipt = argKioskReceipt;
        }
        public MDOM_POS PayPURRequest(PayRecord payrecArg)
        {
            Request.EnumType = PosQAType.PURRequest;
            Request.ResponseReq.PaySumma = payrecArg.Summa;
            //TEST
            if (Request.ResponseReq.PaySumma == "0.00")
            {
                Request.ResponseReq.PaySumma = "1";
            }
            //TEST
            return Request;
        }

        public MDOM_POS GetCancelVOIRequest(PS_ERIP reqArg)
        {
            Request.EnumType = PosQAType.VOIRequest;
            Request.ResponseReq.KioskReceipt = reqArg.ResponseReq.KioskReceipt;
            Request.ResponseReq.PC_ID = reqArg.ResponseReq.PayRecord?.First()?.PC_ID;
            return Request;
        }

        #endregion
    }
}
