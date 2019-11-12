using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlStructureComplat;

namespace Logic
{
    public static class Factory
    {
        public static PS_ERIP PsEripCreate()
        {
            var item = new PS_ERIP();
            item.ResponseReq.TerminalID = StaticMain.Settings.ERIP.TerminalID;
            item.ResponseReq.Version = StaticMain.Settings.ERIP.version;
            return item;
        }
        public static PS_ERIP PsEripHomeCreate()
        {
            var item = PsEripCreate();
            item.ResponseReq.PayCode = StaticMain.Settings.ERIP.HomePaycode;
            item.EnumType = EripQAType.GetPayListRequest;
            return item;
        }

        #region Erip Extensions
        public static PS_ERIP Clear(this PS_ERIP eripArg)
        {
            try
            {
                eripArg.ResponseReq.PayRecord[0].AttrRecord.ForEach(attr =>
                {
                    attr.MinLength = null;
                    attr.MaxLength = null;
                    attr.Min = null;
                    attr.Max = null;
                    attr.Mandatory = null;
                    attr.Lookup = null;
                    attr.Type = null;
                    attr.Hint = null;
                });
            }
            catch (Exception) { }
            return eripArg;
        }
        public static PS_ERIP ConfirmClear(this PS_ERIP eripArg)
        {
            try
            {
                eripArg.ResponseReq.PayRecord[0].AttrRecord = null;
                eripArg.ResponseReq.PayRecord[0].Check = null;
                eripArg.ResponseReq.PayRecord[0].Lookups = null;
                eripArg.ResponseReq.PayRecord[0].PayCommission = null;
                eripArg.ResponseReq.PayRecord[0].Summa = null;
            }
            catch (Exception) { }
            return eripArg;
        }
        public static PS_ERIP Accept(this PS_ERIP eripArg, PS_ERIP arg)
        {
            try
            {
                eripArg.ResponseReq.TerminalID = arg.ResponseReq.TerminalID ?? eripArg.ResponseReq.TerminalID;
                eripArg.ResponseReq.Version = arg.ResponseReq.Version ?? eripArg.ResponseReq.Version;
            }
            catch (Exception) { }
            return eripArg;
        }
        public static PS_ERIP Accept(this PS_ERIP eripArg, MDOM_POS arg)
        {
            try
            {
                eripArg.ResponseReq.PayDate = arg?.ResponseReq?.PayDate;
                eripArg.ResponseReq.KioskReceipt = arg?.ResponseReq?.KioskReceipt;
                eripArg.ResponseReq.PAN = arg?.ResponseReq?.PAN;
                eripArg.ResponseReq.TypePAN = arg?.ResponseReq?.TypePAN;
                eripArg.ResponseReq.KioskReceipt = arg?.ResponseReq?.KioskReceipt;
                var payrec = eripArg.ResponseReq?.PayRecord?.First();
                payrec.PC_ID = arg.ResponseReq?.PC_ID;
            }
            catch (Exception) { }
            return eripArg;
        }
        public static PS_ERIP SetPosError(this PS_ERIP eripArg, MDOM_POS arg)
        {
            try
            {
                if (arg.EnumType == PosQAType.PURResponse)
                { eripArg.EnumType = EripQAType.POSPayResponse; }
                if (arg.EnumType == PosQAType.VOIResponse)
                { eripArg.EnumType = EripQAType.POSCancelResponse; }
                if (arg.EnumType == PosQAType.UnknownResponse)
                { eripArg.EnumType = EripQAType.UnknownResponse; }
                eripArg.ResponseReq.ErrorCode = arg.ResponseReq.ErrorCode;
                eripArg.ResponseReq.ErrorText = arg.ResponseReq.ErrorText;
            }
            catch (Exception) { }
            return eripArg;
        }
#endregion
    }
}
