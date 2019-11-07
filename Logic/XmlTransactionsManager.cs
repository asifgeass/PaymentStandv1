using ExceptionManager;
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
        #region Public Methods
        public async Task<PS_ERIP> NextRequest(object arg=null)
        {
            await CreateNextRequest(arg);
            return await Transaction();
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
                return await Transaction();
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
                var returnReq = await HandleResponseFromUI(arg);
                this.CreateNextPage(returnReq);
            }
        }
        private async Task<PS_ERIP> HandleResponseFromUI(object param)
        {
            PS_ERIP @return = null;
            var rootResponse = list.Page.Response;
            if (rootResponse.EnumType == EripQAType.GetPayListResponse)
            {
                var paylist = rootResponse.ResponseReq.PayRecord;
                if (paylist.Count > 1)
                {
                    if (param is PayRecord)
                    {
                        PayRecord payrecArg = param as PayRecord;
                        var requestCopy = list.Page.Request.Copy();
                        requestCopy.ResponseReq.PayCode = payrecArg.Code;
                        return requestCopy;
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

                            requestCopy.ResponseReq.SessionId = payrecArg.SessionId;
                            requestCopy.ResponseReq.AttrRecord = new List<AttrRecordRequest>();
                            payrecArg.AttrRecord.ForEach(attr =>
                            {
                                var newAttr = new AttrRecordRequest(attr);
                                newAttr.Change = 1;
                                requestCopy.ResponseReq.AttrRecord.Add(newAttr);
                            });
                            return requestCopy;
                        }
                        if (payrecArg.GetPayListType == "0")
                        {
                            MDOM_POS PosReq = new POSManager().PayPURRequest(payrecArg);
                            string request = await POSSerialize(PosReq);
                            MDOM_POS PosRespon = await GetPosResponse(request);
                            string fakeRespon = @"      <MDOM_POS>
        <PURResponse>
          <ErrorCode>0</ErrorCode>
          <PayDate>04/11/2019 15:54:18</PayDate>
          <KioskReceipt>0002</KioskReceipt>
          <PC_ID>000000000000</PC_ID>
          <PAN>522208******0693</PAN>
          <TypePAN>MS</TypePAN>
          <Receipt>
            *****************************
            DEMO MODE
            ONLY FOR TEST
            ДЕМОНСТРАЦИОННЫЙ РЕЖИМ
            ТОЛЬКО ДЛЯ ТЕСТИРОВАНИЯ
            *****************************
            **** TRAINING MODE ****
            ТЕРМИНАЛ: PTS01001
            Торговец: 0000001
            Тестовый терминал
            Расвиком


            КАРТ-ЧЕК: 990003/000001
            ERN: 2
            * ДЛЯ КЛИЕНТА *
            ОПЛАТА
            04.11.2019
            КАРТА: 522208******0693
            Ввод данных - (CL)
            Сумма                           1.00 BYN
            КОД: 00
            ЗАВЕРШЕНО УСПЕШНО
            КОД АВТ.: XXXXXX
            AID: A0000000041010
            APP: Mastercard
          </Receipt>
        </PURResponse>
      </MDOM_POS>"; //FAKE RESPONSE
                            //Ex.Log($"TEST ПОДСТАВА ответа от POS вместо реального");
                            if (PosRespon.ResponseReq.ErrorCode == 0) //УСПЕХ POS
                            {
                                var responCopy = list.Page.Response.Copy();
                                responCopy.EnumType = EripQAType.RunOperationRequest;
                                responCopy.Accept(requestCopy);
                                responCopy.Accept(PosRespon);
                                responCopy.ResponseReq.SessionId = payrecArg.SessionId;
                                var payrec = responCopy.ResponseReq.PayRecord.First();
                                payrec.SessionId = payrecArg.SessionId;
                                Ex.Log("TEST ПОДСТАВА суммы");
                                if (responCopy.ResponseReq.PayRecord[0].Summa == "0.00")
                                {
                                    responCopy.ResponseReq.PayRecord[0].Summa = "1";
                                }
                                responCopy.Clear();
                                return responCopy;
                            }
                            if (PosRespon.ResponseReq.ErrorCode != 0) //ОШИБКА POS
                            {
                                var temp = new PS_ERIP().SetPosError(PosRespon);
                                return temp;
                            }
                        }
                    }
                }
            }
            if(rootResponse.EnumType == EripQAType.RunOperationResponse)
            {
                if (rootResponse.ResponseReq.ErrorCode == 0) // УСПЕХ RunOperRespon
                {
                    //NEVER REACHED
                    ConfirmRequest(rootResponse);
                }
                if (rootResponse.ResponseReq.ErrorCode != 0) // ОШИБКА RunOperRespon
                {
                    //POS CANCEL

                }
            }
            return @return;
        }

        private async Task<string> POSSerialize(MDOM_POS PosArg)
        {
            XDocument reqXml = await SerializationUtil.Serialize(PosArg);
            string request = $"xml={reqXml?.ToString()}";
            return request;
        }

        private async Task<MDOM_POS> GetPosResponse(string argReq)
        {
            XDocument respXml = await PostGetHTTP.PostStringGetXML(POSManager.Url, argReq);
            MDOM_POS respPos = await SerializationUtil.Deserialize<MDOM_POS>(respXml);
            return respPos;
        }
        private async Task CheckRunOperationError(PS_ERIP responArg)
        {
            if (responArg.EnumType == EripQAType.RunOperationResponse)
            {
                if (responArg.ResponseReq.ErrorCode != 0)
                {
                    var RunOpReq = Request; //list.Page.Request
                    if(RunOpReq.EnumType != EripQAType.RunOperationRequest)
                    {
                        string str = "Request is NOT RunOperationRequest.\nCant get PC_ID for Cancel VOI Request";
                        Ex.Throw(str);
                    }
                    MDOM_POS PosReq = new POSManager().CancelVOIRequest(RunOpReq);
                    string request = await POSSerialize(PosReq);
                    MDOM_POS PosRespon = await GetPosResponse(request);
                }
            }
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
        private async Task<PS_ERIP> Transaction()
        {
            string request = await GetEripRequest(list.Page.Request);
            var response = await GetEripResponse(request);
            await HandleResponseWithoutUI(response);            
            return response;
        }

        private async Task HandleResponseWithoutUI(PS_ERIP response)
        {
            CheckForConfirmRequest(response).RunParallel();
            await CheckRunOperationError(response);
        }

        private async Task CheckForConfirmRequest(PS_ERIP response)
        {
            if (response.EnumType == EripQAType.RunOperationResponse)
            {
                Ex.Log($"{nameof(CheckForConfirmRequest)}(): if(RunOperationResponse==true) ROR Error={response.ResponseReq.ErrorCode}");
                if (response.ResponseReq.ErrorCode == 0) // УСПЕХ RunOper
                {
                    bool success = false;
                    int interval = 10;
                    
                    while(!success)
                    {
                        var confirmResp = await ConfirmRequest(response);
                        if(confirmResp.ResponseReq.ErrorCode==0)
                        { success = true; }
                        Ex.Log($"{nameof(CheckForConfirmRequest)}(): Conf Error={confirmResp.ResponseReq.ErrorCode};");
                        await Task.Delay(interval*1000);
                        if (interval<1400) { interval *= 3; }
                    }
                }
            }
            Ex.Log($"{nameof(CheckForConfirmRequest)}() FINISHED.");
        }
        private async Task<PS_ERIP> ConfirmRequest(PS_ERIP RespRunOper)
        {
            PS_ERIP confirmReq = RespRunOper.Copy();
            confirmReq.Accept(this.Request)?.ConfirmClear();
            confirmReq.EnumType = EripQAType.ConfirmRequest;
            confirmReq.ResponseReq.PayRecord[0].KioskReceipt = confirmReq.ResponseReq.KioskReceipt;
            confirmReq.ResponseReq.PayRecord[0].ConfirmCode = "1";
            string req = await GetEripRequest(confirmReq);
            PS_ERIP resp = await GetEripResponse(req);
            return resp;
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
