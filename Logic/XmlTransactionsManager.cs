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
        private XmlHistory list = new XmlHistory();
        private string lastPCID;
        private string lastKioskReceipt;
        private bool isBackToCurrent { get; set; } = false;
        private async Task<PS_ERIP> HandleResponseFromUI(object param)
        {
            Ex.Log($"{nameof(XmlTransactionsManager)}.{nameof(HandleResponseFromUI)}()");
            PS_ERIP @return = null;
            var rootResponse = list.Current.Response;
            if (rootResponse.EnumType == EripQAType.GetPayListResponse)
            {
                var paylist = rootResponse.ResponseReq.PayRecord;
                if (paylist.Count > 1)
                {
                    if (param is PayRecord)
                    {
                        PayRecord payrecArg = param as PayRecord;
                        var requestCopy = list.Current.Request.Copy();
                        requestCopy.ResponseReq.PayCode = payrecArg.Code;
                        return requestCopy;
                    }
                }
                if (paylist.Count == 1)
                {
                    if (param is PayRecord)
                    {
                        var requestCopy = list.Current.Request.Copy();
                        PayRecord payrecArg = param as PayRecord;
                        if (payrecArg.GetPayListType == "1" || payrecArg.GetPayListType == "2")
                        {
                            Ex.Log($"GetPayListType=1/2; SessionID={payrecArg.SessionId}");
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
                            Ex.Log($"GetPayListType=0; SessionID={payrecArg.SessionId}");
                            MDOM_POS PosReq = new POSManager().PayPURRequest(payrecArg);
                            string request = await Serialize(PosReq);
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
                                this.lastPCID = PosRespon?.ResponseReq?.PC_ID;
                                this.lastKioskReceipt = PosRespon?.ResponseReq?.KioskReceipt;
                                var responCopy = list.Current.Response.Copy();
                                responCopy.EnumType = EripQAType.RunOperationRequest;
                                responCopy.Accept(requestCopy);
                                responCopy.Accept(PosRespon);
                                responCopy.ResponseReq.SessionId = payrecArg.SessionId;
                                var payrec = responCopy.ResponseReq.PayRecord.First();
                                payrec.SessionId = payrecArg.SessionId;
                                if (responCopy.ResponseReq.PayRecord[0].Summa == "0.00")
                                {
                                    Ex.Log("TEST ПОДСТАВА суммы 1 вместо 0");
                                    responCopy.ResponseReq.PayRecord[0].Summa = "1";
                                }
                                responCopy.Clear();
                                return responCopy;
                            }
                            if (PosRespon.ResponseReq.ErrorCode != 0) //ОШИБКА POS
                            {
                                var eripResponPosError = new PS_ERIP().SetPosError(PosRespon);
                                return eripResponPosError;
                            }
                        }
                    }
                }
            }
            return @return;
        }
        #region Public Methods
        public async Task<PS_ERIP> NextRequest(object arg=null)
        {
            Ex.Log($"{nameof(XmlTransactionsManager)}.{nameof(NextRequest)}()");
            isBackToCurrent = true;
            await CreateNextRequest(arg);
            isBackToCurrent = false;
            return await Transaction();
        }
        public Task<PS_ERIP> PrevRequest(object arg = null)
        {
            Ex.Log($"{nameof(XmlTransactionsManager)}.{nameof(PrevRequest)}()");
            Ex.Log($"XmlTransactionsManager.{nameof(isBackToCurrent)}={isBackToCurrent}");
            
            EripRequest transaction  = (isBackToCurrent) ? list.Current : list.Previos();
            isBackToCurrent = false;
            return Task.FromResult(transaction.Response);
        }
        public bool IsPrevRequestPossible()
        {
            return list.Current.PrevIndex >= 0 && list.Current.PrevIndex != list.Index;
        }
        public async Task<PS_ERIP> HomeRequest()
        {
            isBackToCurrent = false;
            PS_ERIP req = null; ;
            if (list.Count <= 0)
            {
                await CreateInitialRequest();
            }
            else
            {
                req = list.HomePage().Request;                
            }
            return await Transaction(req);
        }
        #endregion

        #region Private Methods
        private async Task<PS_ERIP> Transaction(PS_ERIP reqArg = null)
        {
            string request = await Serialize(reqArg ?? list.Current.Request);
            var response = await GetEripResponse(request);
            list.SetResponse(response);
            await HandleResponseWithoutUI(response);
            return response;
        }
        private Task CheckRunOperationResponse(PS_ERIP responArg)
        {
            if (responArg.EnumType == EripQAType.RunOperationResponse)
            {
                Ex.Log($"{nameof(XmlTransactionsManager)}.{nameof(CheckRunOperationResponse)}()");
                string confirmArg = "0";
                if (responArg.ResponseReq.ErrorCode == 0)// УСПЕХ RunOper
                {
                    confirmArg = "1";
                    this.lastPCID = null;
                    this.lastKioskReceipt = null;
                    list.Current.SetBackToHome();
                    
                }
                if (responArg.ResponseReq.ErrorCode != 0) //ОШИБКА RunOper
                {
                    confirmArg = "0";
                    CancelPayPOS().RunAsync();                    
                }
                ConfirmTransactionAsync(responArg, confirmArg).RunAsync();
            }
            return Task.CompletedTask;
        }
        private async Task HandleResponseWithoutUI(PS_ERIP response)
        {
            await CheckRunOperationResponse(response);
        }
        private async Task<MDOM_POS> GetPosResponse(string argReq)
        {
            Ex.Log($"{nameof(XmlTransactionsManager)}.{nameof(GetPosResponse)}()");
            XDocument respXml = await PostGetHTTP.PostStringGetXML(POSManager.Url, argReq);
            MDOM_POS respPos = await SerializationUtil.Deserialize<MDOM_POS>(respXml);
            return respPos;
        }
        private async Task ConfirmTransactionAsync(PS_ERIP responArg, string confirmArg)
        {
            var confirmRequest = await GetConfirmRequest(responArg, confirmArg);
            SendConfirmRequest(confirmRequest).RunAsync();
        }
        private async Task CancelPayPOS()
        {
            Ex.Log($"{nameof(XmlTransactionsManager)}.{nameof(CancelPayPOS)}()");
            var RunOpReq = Request; //list.Page.Request
            MDOM_POS CancelPOSReq = null;
            if (RunOpReq.EnumType != EripQAType.RunOperationRequest)
            {
                Ex.Log($"Error: {nameof(CancelPayPOS)} RunOpReq.EnumType != RunOperationRequest");
                CancelPOSReq = new POSManager(lastPCID, lastKioskReceipt).Request;
            }
            else
            {
                CancelPOSReq = new POSManager().GetCancelVOIRequest(RunOpReq);
            }
            string request = await Serialize(CancelPOSReq);
            MDOM_POS CancelRespon = await GetPosResponse(request);
            Ex.Catch(() => HandleCancelPOSResponse(CancelRespon));
        }
        private void HandleCancelPOSResponse(MDOM_POS arg)
        {
            Ex.Log($"{nameof(XmlTransactionsManager)}.{nameof(HandleCancelPOSResponse)}()");
            if (arg?.ResponseReq?.ErrorCode == null)
            {
                string str = $"Error: {nameof(HandleCancelPOSResponse)}(): MDOM_POS response = null";
                Ex.Throw(str);
            }
            if (arg?.ResponseReq?.ErrorCode != 0)
            {
                string str = $"Error: {nameof(HandleCancelPOSResponse)}(): ErrorCode={arg.ResponseReq.ErrorCode}; ErrorText={arg.ResponseReq.ErrorText}";
                Ex.Throw(str);
            }
        }
        private async Task SendConfirmRequest(PS_ERIP confirmRequestArg)
        {
            Ex.Log($"{nameof(XmlTransactionsManager)}.{nameof(SendConfirmRequest)}()");
            bool success = false;
            int interval = 10;

            while (!success)
            {
                if (confirmRequestArg.ResponseReq.ErrorCode == 0)
                { success = true; }
                Ex.Log($"{nameof(SendConfirmRequest)}(): ConfirmResponse Error={confirmRequestArg.ResponseReq.ErrorCode};");
                await Task.Delay(interval * 1000);
                if (interval < 1400) { interval *= 3; }
            }
        }
        private async Task CreateInitialRequest()
        {
            string strxml = GetHardCodeInitialRequest();
            XDocument xml = await PostGetHTTP.XmlLoadAsync(strxml);
            PS_ERIP eripReq = await SerializationUtil.Deserialize<PS_ERIP>(xml);
            this.CreateNextPage(eripReq);
        }
        private PS_ERIP Request => list.Current.Request;
        private void CreateNextPage(PS_ERIP request)
        {
            if (request == null) return;
            list.Next(request);
        }
        private async Task<PS_ERIP> GetEripResponse(string request)
        {
            Ex.Log($"{nameof(XmlTransactionsManager)}.{nameof(GetEripResponse)}()");
            XDocument responseXml = await PostGetHTTP.PostStringGetXML(request);
            PS_ERIP response = await SerializationUtil.Deserialize<PS_ERIP>(responseXml);
            return response;
        }
        private async Task<string> Serialize(MDOM_POS PosArg)
        {
            XDocument reqXml = await SerializationUtil.Serialize(PosArg);
            string request = $"xml={reqXml?.ToString()}";
            return request;
        }
        private async Task<string> Serialize(PS_ERIP argReq)
        {
            XDocument reqXml = await SerializationUtil.Serialize(argReq);
            string request = reqXml?.ToString();
            return request;
        }
        private async Task<PS_ERIP> GetConfirmRequest(PS_ERIP RespRunOper, string argConfirmCode)
        {
            Ex.Log($"{nameof(XmlTransactionsManager)}.{nameof(GetConfirmRequest)}()");
            PS_ERIP confirmReq = new PS_ERIP();
            confirmReq.EnumType = EripQAType.ConfirmRequest;
            confirmReq.ResponseReq.PayRecord = RespRunOper.ResponseReq.PayRecord.Copy();
            confirmReq.Accept(this.Request)?.ConfirmClear();            
            confirmReq.ResponseReq.PayRecord[0].KioskReceipt = RespRunOper.ResponseReq.KioskReceipt.Copy();
            confirmReq.ResponseReq.PayRecord[0].ConfirmCode = argConfirmCode;
            string req = await Serialize(confirmReq);
            PS_ERIP resp = await GetEripResponse(req);
            return resp;
        }
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
