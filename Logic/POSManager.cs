using ExceptionManager;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            if (payrecArg == null) { Ex.Throw<ArgumentNullException>($"{nameof(PayPURRequest)}(): argument {nameof(PayRecord)}=null"); }
            if (string.IsNullOrEmpty(payrecArg.Summa)) { Ex.Throw($"{nameof(PayPURRequest)}(): argument PayRecord.Summa is null or empty"); }
            decimal summa = 0;
            decimal fine = 0;
            decimal commission = 0;
            Ex.Throw($"{nameof(PayPURRequest)}(): не удалось конвертировать PayRecord.Summa='{payrecArg.Summa}' в цифровое значение"
                , () => summa = StringToDecimal(payrecArg.Summa));
            if (payrecArg.Fine != null)
            {
                Ex.Throw($"{nameof(PayPURRequest)}(): не удалось конвертировать PayRecord.Fine='{payrecArg.Summa}' в цифровое значение"
                    , () => fine = StringToDecimal(payrecArg.Fine));
            }
            if (payrecArg.PayCommission != null)
            {
                Ex.Throw($"{nameof(PayPURRequest)}(): не удалось конвертировать PayRecord.PayCommission='{payrecArg.Summa}' в цифровое значение"
                  , () => commission = StringToDecimal(payrecArg.PayCommission));
            }
            decimal PayAllPOS = summa + fine + commission;
            //TEST
            if (PayAllPOS == 0)
            {
                PayAllPOS = 1.11M;
            }
            //TEST
            Request.ResponseReq.PaySumma = PayAllPOS.ToString().Replace(',','.');
            Request.EnumType = PosQAType.PURRequest;

            return Request;
        }

        private static decimal StringToDecimal(string argString)
        {
            return decimal.Parse(argString.Replace(',', '.'), NumberStyles.Currency, CultureInfo.InvariantCulture);
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
