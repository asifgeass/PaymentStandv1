using Logic.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XmlStructureComplat;

namespace Logic
{
    public class PagesManager
    {
        #region fields
        private List<DynamicPage> pages = new List<DynamicPage>();
        private int currentIndex = -1;
        #endregion

        #region Properties
        public int Count { get => pages.Count; }
        #endregion

        #region Public Methods
        public async Task<PS_ERIP> NextRequest(object arg=null)
        {
            string request = await GetRequest(arg);
            return await SendRequest(request);
        }
        #endregion
        #region Private Methods
        private async Task<string> GetRequest(object arg = null)
        {
            string request = null;
            if (pages.Count <= 0)
            {
                await CreateInitialPage();
            }
            else 
            { 
                CreateNextPage(arg); 
            }
            PS_ERIP reqClass = this.Page.Request;
            XDocument reqXml = await SerializationUtil.Serialize(reqClass);
            request = reqXml.ToStringFull();
            return request;
        }
        private void CreateNextPage(object param)
        {
            var rootResponse = this.Page.Response;
            var paylist = rootResponse.GetListResponse.PayRecord;
            if (paylist.Count > 1)
            {
                if (param is PayRecord)
                {
                    PayRecord payrecArg = param as PayRecord;
                    var reqErip = Page.Request.Copy();
                    reqErip.GetListResponse.PayCode = payrecArg.Code;
                    this.CreateNextPage(reqErip);
                }
            }
        }
        private async Task CreateInitialPage()
        {
            string strxml = GetHardCodeInitialRequest();
            XDocument xml = await PostGetHTTP.XmlLoadAsync(strxml);
            PS_ERIP eripReq = await SerializationUtil.Deserialize<PS_ERIP>(xml);
            this.CreateNextPage(eripReq);
        }
        private DynamicPage Page 
            => (currentIndex < 0 || currentIndex >= pages.Count) 
            ? null : pages[currentIndex];
        private DynamicPage PrevPage
            => (currentIndex < 1 || currentIndex >= pages.Count)
            ? null : pages[currentIndex-1];

        private PS_ERIP Request => this.Page.Request;
        private void CreateNextPage(PS_ERIP request)
        {
            var page = new DynamicPage();
            page.Request = request;
            pages.Add(page);
            currentIndex++;
        }
        private async Task<PS_ERIP> SendRequest(string request)
        {
            XDocument responceXml = await PostGetHTTP.PostStringGetXML(request);
            PS_ERIP responce = await SerializationUtil.Deserialize<PS_ERIP>(responceXml);
            this.Page.Response = responce;
            return responce;
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
