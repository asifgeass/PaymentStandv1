using ExceptionManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.IO;
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
        private POSManager lastPOSTransaction = new POSManager();
        private bool isBackToCurrent { get; set; } = false;
        private async Task<PS_ERIP> HandlePayRecordParam(PayRecord payrecArg, PS_ERIP requestArg=null)
        {
            PS_ERIP @return = null;
            if (payrecArg == null) Ex.Throw<ArgumentNullException>($"{nameof(HandlePayRecordParam)}(): PayRecord argument = null");
            PS_ERIP requestCopy = requestArg ?? list.Current.Request.Copy();
            if (payrecArg.GetPayListType == "1" || payrecArg.GetPayListType == "2")
            {
                Ex.Log($"{nameof(HandlePayRecordParam)}(): GetPayListType=1/2; SessionID={payrecArg.SessionId}");
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
                Ex.Try(false, ()=> payrecArg.Summa = payrecArg.Summa.Replace(",","."));
                lastPOSTransaction.CreatePayPosRequest(payrecArg);
                string request = await Serialize(lastPOSTransaction.Request);
                MDOM_POS PosRespon = await GetPosResponse(request);
                lastPOSTransaction.Response = PosRespon;
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
                    responCopy.Accept(PosRespon).RunOpRequestClear(); ;
                    responCopy.ResponseReq.SessionId = payrecArg.SessionId;
                    var payrec = responCopy.ResponseReq.PayRecord.FirstOrDefault();
                    payrec.SessionId = payrecArg.SessionId;
                    Ex.Try(false, () => payrec.Summa = payrec.Summa.Replace(",", "."));
                    responCopy.ClearAttrRecords();
                    return responCopy;
                }
                if (PosRespon.ResponseReq.ErrorCode != 0) //ОШИБКА POS
                {
                    var eripResponPosError = Factory.PsEripCreate().SetPosError(PosRespon);
                    return eripResponPosError;
                }
            }
            return @return;
        }
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
                    if (param is PS_ERIP)
                    {
                        PS_ERIP eripArg = param as PS_ERIP;
                        PS_ERIP requestCopy = null;
                        //if (eripArg.ResponseReq.PaySumma != null)
                        //{
                        //    requestCopy = list.Current.Request.Copy();
                        //    requestCopy.ResponseReq.PaySumma = eripArg.ResponseReq.PaySumma;
                        //}
                        PayRecord payrecArg = eripArg.ResponseReq.PayRecord.FirstOrDefault();
                        return await HandlePayRecordParam(payrecArg, requestCopy);
                    }
                    if (param is PayRecord)
                    {
                        PayRecord payrecArg = param as PayRecord;
                        return await HandlePayRecordParam(payrecArg);
                    }
                }
                if (paylist.Count <= 0) { Ex.Throw($"{nameof(HandleResponseFromUI)}(): paylist.Count <= 0)"); }
            }
            return @return;
        }
        #region Public Methods
        public async Task<PS_ERIP> NextRequest(object arg=null)
        {
            Ex.Log($"{nameof(XmlTransactionsManager)}.{nameof(NextRequest)}()");
            isBackToCurrent = true;
            await CheckForNULL();
            PS_ERIP eripBody = await HandleResponseFromUI(arg);
            isBackToCurrent = false;
            list.CreateNextPage(eripBody);
            if (eripBody.EnumType == EripQAType.POSPayResponse || eripBody.EnumType== EripQAType.POSCancelResponse)
            { return eripBody; }
            return await Transaction();
        }
        public async Task<PS_ERIP> PrevRequest(object arg = null)
        {
            Ex.Log($"XmlTransactionsManager.{nameof(PrevRequest)}()");
            Ex.Log($"XmlTransactionsManager.{nameof(isBackToCurrent)}={isBackToCurrent}");
            bool isTransactionClosed = list?.Current?.Response?.ResponseReq?.PayRecord?.FirstOrDefault()?.GetPayListType == "0";
            EripRequest transaction  = (isBackToCurrent) ? list.Current : list.Previos();
            isBackToCurrent = false;
            var respon = transaction.Response;

            if (isTransactionClosed)
            { //GetPayListType==0
                string sessionID = respon.ResponseReq.PayRecord?.FirstOrDefault()?.SessionId;
                bool isRespon = true; //start checking with Response
                short countChecks = 1; // checked req + resp=2
                while (!string.IsNullOrEmpty(sessionID))
                { //on first come check request  
                    if (countChecks >= 2)
                    {
                        transaction = list.Previos();
                        countChecks = 0;
                    }
                    isRespon = !isRespon; 
                    sessionID = (isRespon) //on first come: isRespon=false
                        ? transaction.Response.ResponseReq.PayRecord?.FirstOrDefault()?.SessionId 
                        : transaction.Request.ResponseReq.SessionId;
                    countChecks++;
                }//on exit sessionID==null
                respon = isRespon ? transaction.Response : await Transaction(transaction.Request);
            }
            
            return respon;
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
        private Task CheckRunOperationResponse(PS_ERIP responArg)
        {
            if (responArg.EnumType == EripQAType.RunOperationResponse)
            {
                Ex.Log($"{nameof(XmlTransactionsManager)}.{nameof(CheckRunOperationResponse)}()");
                string confirmArg = "0";
                if (responArg.ResponseReq.ErrorCode == 0)// УСПЕХ RunOper
                {
                    confirmArg = "1";
                    list.Current.SetBackToHome();
                    string CheckRunOpResp = AssembleRunOpResponCheck(responArg);
                    this.lastPCID = null;
                    this.lastKioskReceipt = null;
                    RunPrintingsAnotherThread(CheckRunOpResp);
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
        #endregion

        #region Printing
        private async Task RunPrintingsAnotherThread(string CheckRunOpResp)
        {
            try
            {
                Font font = SetFont();
                Print(CheckRunOpResp, font);
                await Task.Delay(500);
                Print(lastPOSTransaction.Response.ResponseReq.Receipt, font);
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        }        
        private void Print(string textArg, Font font)
        {
            Ex.TryLog(() =>
            {
                Ex.Log($"XmlTransactionsManager.Print() isNull={textArg == null}");
                PrintDocument printDocument = new PrintDocument();                
                printDocument.PrintPage += (s, e) => e.Graphics.DrawString(textArg, font, Brushes.Black, 0, 0);
                printDocument.Print();
                Ex.Log($"XmlTransactionsManager.Print() END DONE");
            });
        }
        private static Font SetFont()
        {
            Font font = new Font(FontFamily.GenericMonospace, 8.25f);
            //Font font = new Font("Courier", 8.25f, FontStyle.Bold);
            string path = $@"{AppDomain.CurrentDomain.BaseDirectory}\Resources\Courier.ttf";
            Ex.TryLog(() =>
            {
                bool isFile = File.Exists(path);
                if (isFile)
                {
                    PrivateFontCollection privateFontCollection = new PrivateFontCollection();
                    privateFontCollection.AddFontFile(path);
                    var fontFam = privateFontCollection.Families.FirstOrDefault();
                    font = new Font(fontFam, 8.25f);
                }
                else Ex.Log($"Font file not found={path}");
            });
            Ex.Log($"XmlTransactionsManager.SetFont(): {font}");
            return font;
        }
        private string AssembleRunOpResponCheck(PS_ERIP responArg)
        {
            var CheckRunOpResp = new StringBuilder();
            try
            {
                responArg.ResponseReq.PayRecord.FirstOrDefault()?.Check?.CheckHeader?.CheckLine?.ForEach(x =>
                {
                    CheckRunOpResp.AppendLine(x?.Value);
                    Ex.Try(x?.Value != null, () =>
                    {
                        if (x.Value.Contains("Дата платежа"))
                        {
                            var mustLength = x.Value.Length;
                            var str = "Код платежа:";
                            var emptyLength = mustLength - str.Length - lastKioskReceipt.Length;
                            if (emptyLength >= 0)
                            {
                                var emptyStr = "                                                                           "
                                    .trySubstring(0, emptyLength);
                                CheckRunOpResp.AppendLine($"{str}{emptyStr}{lastKioskReceipt}");
                            }
                        }
                    });
                });
            }
            catch (Exception ex)
            {
                ex.Log();
            }

            return CheckRunOpResp?.ToString();
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
        private async Task HandleResponseWithoutUI(PS_ERIP response)
        {
            await CheckRunOperationResponse(response);
        }
        private async Task<MDOM_POS> GetPosResponse(string argReq)
        {
            Ex.Log($"{nameof(XmlTransactionsManager)}.{nameof(GetPosResponse)}()");
            XDocument respXml = await PostGetHTTP.PostStringGetXML(StaticMain.Settings.Terminal_MdomPOS.url, argReq);
            MDOM_POS respPos = await SerializationUtil.Deserialize<MDOM_POS>(respXml);
            return respPos;
        }
        private async Task CancelPayPOS()
        {
            Ex.Log($"{nameof(XmlTransactionsManager)}.{nameof(CancelPayPOS)}()");
            var RunOpReq = Request; //list.Page.Request
            MDOM_POS CancelPOSReq = null;
            if (RunOpReq.EnumType != EripQAType.RunOperationRequest)
            {
                Ex.Log($"Error: {nameof(CancelPayPOS)} RunOpReq.EnumType != RunOperationRequest");
                CancelPOSReq = lastPOSTransaction.GetCancelVOIRequest(lastPCID, lastKioskReceipt);
            }
            else
            {
                CancelPOSReq = lastPOSTransaction.GetCancelVOIRequest(RunOpReq);
            }
            string request = await Serialize(CancelPOSReq);
            lastPOSTransaction.Response = await GetPosResponse(request);
            Ex.Catch(() => HandleCancelPOSResponse(lastPOSTransaction.Response));
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
        private async Task ConfirmTransactionAsync(PS_ERIP responArg, string confirmArg)
        {
            PS_ERIP confirmRequest = await GetConfirmRequest(responArg, confirmArg);
            SendConfirmRequest(confirmRequest).RunAsync();
        }
        private async Task SendConfirmRequest(PS_ERIP confirmRequestArg)
        {
            Ex.Log($"{nameof(XmlTransactionsManager)}.{nameof(SendConfirmRequest)}()");
            bool success = false;
            int interval = 10;
            string request = await Serialize(confirmRequestArg);

            while (!success)
            {                
                var response = await GetEripResponse(request);
                if (response.ResponseReq.ErrorCode == 0)
                { success = true; }
                Ex.Log($"{nameof(SendConfirmRequest)}(): ConfirmResponse Error={response.ResponseReq.ErrorCode};");
                if (confirmRequestArg?.ResponseReq?.PayRecord?.FirstOrDefault()?.PaymentID == null) break;
                await Task.Delay(interval * 1000);
                if (interval < 1400) { interval *= 3; }
            }
        }
        private async Task<PS_ERIP> GetConfirmRequest(PS_ERIP RespRunOper, string argConfirmCode)
        {
            Ex.Log($"{nameof(XmlTransactionsManager)}.{nameof(GetConfirmRequest)}()");
            PS_ERIP confirmReq = Factory.PsEripCreate();
            confirmReq.EnumType = EripQAType.ConfirmRequest;
            confirmReq.ResponseReq.PayRecord = RespRunOper.ResponseReq.PayRecord.Copy();
            confirmReq.Accept(this.Request)?.ConfirmClear();
            confirmReq.ResponseReq.PayRecord[0].KioskReceipt = RespRunOper.ResponseReq.KioskReceipt.Copy();
            confirmReq.ResponseReq.PayRecord[0].ConfirmCode = argConfirmCode;
            //string req = await Serialize(confirmReq);
            //PS_ERIP resp = await GetEripResponse(req);
            return confirmReq;
        }
        private async Task CreateInitialRequest()
        {
            PS_ERIP homeErip = Factory.PsEripHomeCreate();
            list.CreateNextPage(homeErip);
        }
        private PS_ERIP Request => list.Current.Request;
        private async Task<PS_ERIP> GetEripResponse(string request)
        {
            Ex.Log($"{nameof(XmlTransactionsManager)}.{nameof(GetEripResponse)}()");
            XDocument responseXml = await PostGetHTTP.PostStringGetXML(StaticMain.Settings.ERIP.url, request);
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
        private async Task CheckForNULL()
        {
            if (list.Count <= 0)
            {
                await CreateInitialRequest();
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
