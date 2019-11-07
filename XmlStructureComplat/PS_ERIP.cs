using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlStructureComplat
{
    [XmlRoot]
    public class PS_ERIP
    {
        [XmlIgnore]
        public EripQAType EnumType;

        [XmlChoiceIdentifier("EnumType")]
        [XmlElement("GetPayListResponse")]
        [XmlElement("GetPayListRequest")]
        [XmlElement("RunOperationRequest")]
        [XmlElement("ConfirmRequest")]
        [XmlElement("RunOperationResponse")]
        [XmlElement("UnknownResponse")]
        [XmlElement("ConfirmResponse")]
        [XmlElement("POSPayError")]
        [XmlElement("POSCancelError")]
        public GetPayResponse ResponseReq { get; set; } = new GetPayResponse();
        public PS_ERIP Clear()
        {
            try
            {
                this.ResponseReq.PayRecord[0].AttrRecord.ForEach(attr =>
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
            return this;
        }
        public PS_ERIP ConfirmClear()
        {
            try
            {
                this.ResponseReq.PayRecord[0].AttrRecord = null;
                this.ResponseReq.PayRecord[0].Check = null;
                this.ResponseReq.PayRecord[0].Lookups = null;
                this.ResponseReq.PayRecord[0].PayCommission = null;
                this.ResponseReq.PayRecord[0].Summa = null;
            }
            catch (Exception) { }
            return this;
        }
        public PS_ERIP Accept(PS_ERIP arg)
        {
            try
            {
                ResponseReq.TerminalID = arg.ResponseReq.TerminalID ?? ResponseReq.TerminalID;
                ResponseReq.Version = arg.ResponseReq.Version ?? ResponseReq.Version;
            }
            catch (Exception) { }
            return this;
        }
        public PS_ERIP Accept(MDOM_POS arg)
        {
            try
            {
                ResponseReq.PayDate = arg?.ResponseReq?.PayDate;
                ResponseReq.KioskReceipt = arg?.ResponseReq?.KioskReceipt;
                ResponseReq.PAN = arg?.ResponseReq?.PAN;
                ResponseReq.TypePAN = arg?.ResponseReq?.TypePAN;
                ResponseReq.KioskReceipt = arg?.ResponseReq?.KioskReceipt;
                var payrec = ResponseReq?.PayRecord?.First();
                payrec.PC_ID = arg.ResponseReq?.PC_ID;
            }
            catch (Exception) { }
            return this;
        }

        public PS_ERIP SetPosError(MDOM_POS arg)
        {
            try
            {
                if(arg.EnumType == PosQAType.PURResponse)
                { this.EnumType = EripQAType.POSPayError; }
                if (arg.EnumType == PosQAType.VOIResponse)
                { this.EnumType = EripQAType.POSCancelError; }
                if (arg.EnumType == PosQAType.UnknownResponse)
                { this.EnumType = EripQAType.UnknownResponse; }
                this.ResponseReq.ErrorCode = arg.ResponseReq.ErrorCode;
                this.ResponseReq.ErrorText = arg.ResponseReq.ErrorText;
            }
            catch (Exception) { }
            return this;
        }
    }

    [XmlType(IncludeInSchema = false)]
    public enum EripQAType
    {
        GetPayListResponse,
        GetPayListRequest,
        RunOperationRequest,
        ConfirmRequest,
        RunOperationResponse,
        UnknownResponse,
        POSPayError,
        POSCancelError,
        ConfirmResponse
    }
}
