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
        private async Task<PS_ERIP> GetAndSend()
        {
            string request = await GetEripRequest();
            return await GetEripResponse(request);
        }
        private async Task<string> GetEripRequest()
        {    
            PS_ERIP reqClass = list.Page.Request;
            XDocument reqXml = await SerializationUtil.Serialize(reqClass);
            string request = reqXml?.ToStringFull();
            return request;
        }
        private async Task CreateNextRequest(object arg)
        {
            if (list.Count <= 0)
            {
                await CreateInitialRequest();
            }
            else
            {
                CreateNextPage(arg);
            }
        }
        private async Task CreateNextPage(object param)
        {
            var rootResponse = list.Page.Response;
            var paylist = rootResponse.GetListResponse.PayRecord;
            PS_ERIP requestReturn = null;
            if (paylist.Count > 1)
            {
                if (param is PayRecord)
                {
                    PayRecord payrecArg = param as PayRecord;
                    var requestCopy = list.Page.Request.Copy();
                    requestCopy.GetListResponse.PayCode = payrecArg.Code;
                    requestReturn = requestCopy;
                }
            }
            if (paylist.Count == 1)
            {
                if (param is PayRecord)
                {
                    PayRecord payrecArg = param as PayRecord;
                    var newRequest = list.Page.Request.Copy();
                    newRequest.GetListResponse.SessionId = payrecArg.SessionId;

                    if(payrecArg.GetPayListType == "1" || payrecArg.GetPayListType == "2")
                    {
                        newRequest.GetListResponse.AttrRecord = new List<AttrRecordRequest>();
                        payrecArg.AttrRecord.ForEach(attr =>
                        {
                            var newAttr = new AttrRecordRequest(attr);
                            newAttr.Change = 1;
                            newRequest.GetListResponse.AttrRecord.Add(newAttr);
                        });
                        requestReturn = newRequest;
                    }
                    if(payrecArg.GetPayListType=="0")
                    {
                        string request = await GetPosRequest(payrecArg);
                        MDOM_POS resp = await GetPosResponse(request);
                        if(resp.Root2.ErrorCode==0) //УСПЕХ оплаты
                        {
                            //build RunOperReq
                        }
                        if (resp.Root2.ErrorCode != 0) //ОШИБКА оплаты
                        {
                            //ошибка в ЮИ
                        }
                    }
                }
            }
            this.CreateNextPage(requestReturn);
        }
        private async Task<string> GetPosRequest(PayRecord payrecArg)
        {
            QAMdomPOS pos = new QAMdomPOS(payrecArg);
            XDocument reqXml = await SerializationUtil.Serialize(pos.Request);
            string request = $"xml={reqXml?.ToStringFull()}";
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
