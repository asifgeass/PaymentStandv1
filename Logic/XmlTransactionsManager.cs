using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XmlStructureComplat;
using XmlStructureComplat.MDOM_POS;

namespace Logic
{
    public class XmlTransactionsManager
    {
        #region fields
        private XmlHistory list = new XmlHistory();
        #endregion
        #region Public Methods
        public async Task<PS_ERIP> NextRequest(object arg=null)
        {
            await CreateNextRequest(arg);
            return await GetAndSend();
        }
        public async Task<PS_ERIP> PrevRequest(object arg = null)
        {            
            return list.Previos().Response;
        }
        public bool IsPrevRequestPossible()
        {
            return list.Page.PrevIndex >= 0 && list.Page.PrevIndex != list.Index;
        }
        public async Task<PS_ERIP> HomeRequest()
        {
            if (list.Count <= 0)
            {
                await CreateInitialRequest();
                return await GetAndSend();
            }
            else
            {
                return list.HomePage().Response;
            }
        }
        #endregion
        #region Private Methods

        private async Task CreateNextRequest(object arg)
        {
            if (list.Count <= 0)
            {
                await CreateInitialRequest();
            }
            else
            {
                await CreateNextPage(arg);
            }
        }
        private async Task CreateNextPage(object param)
        {
            PS_ERIP returnReq = null;
            var rootResponse = list.Page.Response;
            if (rootResponse.EnumType == EripQAType.GetPayListResponse)
            {
                var paylist = rootResponse.RootQAType.PayRecord;
                if (paylist.Count > 1)
                {
                    if (param is PayRecord)
                    {
                        PayRecord payrecArg = param as PayRecord;
                        var requestCopy = list.Page.Request.Copy();
                        requestCopy.RootQAType.PayCode = payrecArg.Code;
                        returnReq = requestCopy;
                    }
                }
                if (paylist.Count == 1)
                {
                    if (param is PayRecord)
                    {
                        var requestCopy = list.Page.Request.Copy();
                        PayRecord payrecArg = param as PayRecord;
                        if (payrecArg.GetPayListType == "1" || payrecArg.GetPayListType == "2")
                        {

                            requestCopy.RootQAType.SessionId = payrecArg.SessionId;
                            requestCopy.RootQAType.AttrRecord = new List<AttrRecordRequest>();
                            payrecArg.AttrRecord.ForEach(attr =>
                            {
                                var newAttr = new AttrRecordRequest(attr);
                                newAttr.Change = 1;
                                requestCopy.RootQAType.AttrRecord.Add(newAttr);
                            });
                            returnReq = requestCopy;
                        }
                        if (payrecArg.GetPayListType == "0")
                        {
                            string request = await GetPosRequest(payrecArg);
                            MDOM_POS respPos = await GetPosResponse(request);
                            if (respPos.RootQAType.ErrorCode == 0) //УСПЕХ POS
                            {
                                returnReq = list.Page.Response.Copy();
                                returnReq.EnumType = EripQAType.RunOperationRequest;
                                returnReq.RootQAType.TerminalID = requestCopy.RootQAType.TerminalID;
                                returnReq.RootQAType.Version = requestCopy.RootQAType.Version;
                                returnReq.RootQAType.SessionId = payrecArg.SessionId;
                                returnReq.RootQAType.PayDate = respPos.RootQAType.PayDate;
                                returnReq.RootQAType.KioskReceipt = respPos.RootQAType.KioskReceipt;
                                returnReq.RootQAType.PAN = respPos.RootQAType.PAN;
                                returnReq.RootQAType.TypePAN = respPos.RootQAType.TypePAN;
                                returnReq.RootQAType.KioskReceipt = respPos.RootQAType.KioskReceipt;
                                returnReq.RootQAType.PayRecord.First().PC_ID = respPos.RootQAType.PC_ID;
                                returnReq.Clear();
                                returnReq.RootQAType.PayRecord.First().SessionId = payrecArg.SessionId;

                                //string ReqRunOper = await GetEripRequest(newRequest);
                                //PS_ERIP RespRunOper = await GetEripResponse(ReqRunOper);

                                //if (RespRunOper.RootQAType.ErrorCode==0) // УСПЕХ RunOper
                                //{
                                //    returnReq = RespRunOper;
                                //    //Успех в ЮИ юзеру
                                //    ConfirmRequest(RespRunOper);
                                //}
                            }
                            if (respPos.RootQAType.ErrorCode != 0) //ОШИБКА POS
                            {
                                //ошибка в ЮИ
                            }
                        }
                    }
                }
            }
            if(rootResponse.EnumType == EripQAType.RunOperationResponse)
            {
                if (rootResponse.RootQAType.ErrorCode == 0) // УСПЕХ RunOper
                {
                    //Успех в ЮИ юзеру
                    ConfirmRequest(rootResponse);
                }
            }
            this.CreateNextPage(returnReq);
        }

        private async Task ConfirmRequest(PS_ERIP RespRunOper)
        {
            PS_ERIP confirmReq = RespRunOper.Copy();
            confirmReq.EnumType = EripQAType.ConfirmRequest;
            confirmReq.RootQAType.PayRecord[0].KioskReceipt = confirmReq.RootQAType.KioskReceipt;
            confirmReq.RootQAType.PayRecord[0].ConfirmCode = "1";
            string req = await GetEripRequest(confirmReq);
            PS_ERIP resp = await GetEripResponse(req);
        }

        private async Task<string> GetPosRequest(PayRecord payrecArg)
        {
            QAMdomPOS pos = new QAMdomPOS(payrecArg);
            XDocument reqXml = await SerializationUtil.Serialize(pos.Request);
            string request = $"xml={reqXml?.ToString()}";
            return request;
        }
        private async Task<MDOM_POS> GetPosResponse(string argReq)
        {
            XDocument respXml = await PostGetHTTP.PostStringGetXML(QAMdomPOS.Url, argReq);
            MDOM_POS respPos = await SerializationUtil.Deserialize<MDOM_POS>(respXml);
            return respPos;
        }

        private async Task CreateInitialRequest()
        {
            string strxml = GetHardCodeInitialRequest();
            XDocument xml = await PostGetHTTP.XmlLoadAsync(strxml);
            PS_ERIP eripReq = await SerializationUtil.Deserialize<PS_ERIP>(xml);
            this.CreateNextPage(eripReq);
        }
        private PS_ERIP Request => list.Page.Request;
        private void CreateNextPage(PS_ERIP request)
        {
            if (request == null) return;
            list.Next(request);
        }
        private async Task<PS_ERIP> GetEripResponse(string request)
        {
            XDocument responseXml = await PostGetHTTP.PostStringGetXML(request);
            Debug.WriteLine(responseXml.ToString());
            PS_ERIP response = await SerializationUtil.Deserialize<PS_ERIP>(responseXml);
            list.Page.Response = response;
            return response;
        }
        private async Task<string> GetEripRequest(PS_ERIP argReq)
        {
            XDocument reqXml = await SerializationUtil.Serialize(argReq);
            string request = reqXml?.ToString();
            return request;
        }
        private async Task<PS_ERIP> GetAndSend()
        {
            string request = await GetEripRequest(list.Page.Request);
            return await GetEripResponse(request);
        }
        private string GetHardCodeInitialRequest()
        {
            string  str=
                @"
                <PS_ERIP>
                  <GetPayListRequest>
                    <TerminalID>TEST_TERMINAL</TerminalID>
                    <Version>3</Version>
                    <PayCode>400</PayCode>
                  </GetPayListRequest>
                </PS_ERIP>
                ";
            return str;
        }
        #endregion
    }
}
