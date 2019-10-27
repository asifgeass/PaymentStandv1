using Logic.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        public async Task<PS_ERIP> NextRequest()
        {
            string request = await GetRequest();
            return await SendRequest(request);
        }
        #endregion

        #region Private Methods
        private async Task<string> GetRequest()
        {
            string request = null;
            if (pages.Count == 0)
            {
                await CreateInitialPage();
            }
            PS_ERIP reqClass = this.GetCurrentPageRequest();
            XDocument reqXml = await SerializationUtil.Serialize(reqClass);
            request = reqXml.ToStringFull();
            return request;
        }

        private async Task CreateInitialPage()
        {
            string strxml = GetHardCodeInitialRequest();
            XDocument xml = await PostGetHTTP.XmlLoadAsync(strxml);
            PS_ERIP eripReq = await SerializationUtil.Deserialize<PS_ERIP>(xml);
            this.CreateNewPage(eripReq);
        }

        private PS_ERIP GetCurrentPageRequest()
        {
            var page = this.GetPage();
            return page.xmlRequest;
        }
        private DynamicPage GetPage()
        {
            if (currentIndex < 0 || currentIndex >= pages.Count)
            {
                return null;
            }
            return pages[currentIndex];
        }
        private void CreateNewPage(PS_ERIP request)
        {
            var page = new DynamicPage();
            page.xmlRequest = request;
            pages.Add(page);
            currentIndex++;
        }

        private async Task<PS_ERIP> SendRequest(string request)
        {
            XDocument responceXml = await PostGetHTTP.PostStringGetXML(request);
            PS_ERIP responce = await SerializationUtil.Deserialize<PS_ERIP>(responceXml);
            this.GetPage().xmlBody = responce;
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
