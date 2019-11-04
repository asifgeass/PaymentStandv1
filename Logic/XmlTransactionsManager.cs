using Logic.XML;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XmlStructureComplat;

namespace Logic
{
    public class XmlTransactionsManager
    {
        #region fields
        private XmlHistory list = new XmlHistory();
        #endregion

        #region Properties
        //public int Count { get => list.Count; }
        #endregion

        #region Public Methods
        public async Task<PS_ERIP> NextRequest(object arg=null)
        {
            string request = await GetRequestBody(arg);
            return await SendRequestGetResponse(request);
        }
        public async Task<PS_ERIP> PrevRequest(object arg = null)
        {
            return list.Prev().Response;
        }
        public async Task<PS_ERIP> HomeRequest(object arg = null)
        {
            await CreateInitialRequest();
            string request = await GetRequestBody(arg);
            return await SendRequestGetResponse(request);
        }
        #endregion
        #region Private Methods
        private async Task<string> GetRequestBody(object arg = null)
        {
            string request = null;
            if (list.Count <= 0)
            {
                await CreateInitialRequest();
            }
            else 
            { 
                CreateNextPage(arg); 
            }
            PS_ERIP reqClass = list.Page.Request;
            XDocument reqXml = await SerializationUtil.Serialize(reqClass);
            request = reqXml.ToStringFull();
            return request;
        }
        private void HomeRequest()
        {

        }
        private void CreateNextPage(object param)
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

                    }
                }
            }
            this.CreateNextPage(requestReturn);
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
            list.Add(request);
        }
        private async Task<PS_ERIP> SendRequestGetResponse(string request)
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
