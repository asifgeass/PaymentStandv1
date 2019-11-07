using ExceptionManager;
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
                var returnReq = await AnalyzeResponse(arg);
                this.CreateNextPage(returnReq);
            }
        }
        private async Task<PS_ERIP> AnalyzeResponse(object param)
        {
            PS_ERIP @return = null;
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

                            requestCopy.RootQAType.SessionId = payrecArg.SessionId;
                            requestCopy.RootQAType.AttrRecord = new List<AttrRecordRequest>();
                            payrecArg.AttrRecord.ForEach(attr =>
                            {
                                var newAttr = new AttrRecordRequest(attr);
                                newAttr.Change = 1;
                                requestCopy.RootQAType.AttrRecord.Add(newAttr);
                            });
                            return requestCopy;
                        }
                        if (payrecArg.GetPayListType == "0")
                        {
                            string request = await GetPosRequest(payrecArg);
                            MDOM_POS respPos = await GetPosResponse(request);
                            string responseStr = @"      <MDOM_POS>
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
                            //Debug.WriteLine($"TEST ПОДСТАВА ответа от POS вместо реального");
                            if (respPos.ResponseReq.ErrorCode == 0) //УСПЕХ POS
                            {
                                var respCopy = list.Page.Response.Copy();
                                respCopy.EnumType = EripQAType.RunOperationRequest;
                                respCopy.RootQAType.TerminalID = requestCopy.RootQAType.TerminalID;
                                respCopy.RootQAType.Version = requestCopy.RootQAType.Version;
                                respCopy.RootQAType.SessionId = payrecArg.SessionId;
                                respCopy.RootQAType.PayDate = respPos.ResponseReq.PayDate;
                                respCopy.RootQAType.KioskReceipt = respPos.ResponseReq.KioskReceipt;
                                respCopy.RootQAType.PAN = respPos.ResponseReq.PAN;
                                respCopy.RootQAType.TypePAN = respPos.ResponseReq.TypePAN;
                                respCopy.RootQAType.KioskReceipt = respPos.ResponseReq.KioskReceipt;
                                var payrec = respCopy.RootQAType.PayRecord.First();
                                payrec.PC_ID = respPos.ResponseReq.PC_ID;
                                payrec.SessionId = payrecArg.SessionId;
                                //TEST
                                Trace.WriteLine("TEST ПОДСТАВА суммы");
                                if (respCopy.RootQAType.PayRecord[0].Summa == "0.00")
                                {
                                    respCopy.RootQAType.PayRecord[0].Summa = "1";
                                }
                                //TEST
                                respCopy.Clear();
                                return respCopy;

                                //if (RespRunOper.RootQAType.ErrorCode==0) // УСПЕХ RunOper
                                //{
                                //    returnReq = RespRunOper;
                                //    //Успех в ЮИ юзеру
                                //    ConfirmRequest(RespRunOper);
                                //}
                            }
                            if (respPos.ResponseReq.ErrorCode != 0) //ОШИБКА POS
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
            return @return;
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
            var response = await GetEripResponse(request);
            CheckForConfirmRequest(response);
            return response;
        }

        private async Task CheckForConfirmRequest(PS_ERIP response)
        {
            if (response.EnumType == EripQAType.RunOperationResponse)
            {
                Ex.Log($"{nameof(CheckForConfirmRequest)}(): if(RunOperationResponse==true) ROR Error={response.RootQAType.ErrorCode}");
                if (response.RootQAType.ErrorCode == 0) // УСПЕХ RunOper
                {
                    bool success = false;
                    int interval = 10;
                    
                    while(!success)
                    {
                        var confirmResp = await ConfirmRequest(response);
                        if(confirmResp.RootQAType.ErrorCode==0)
                        { success = true; }
                        Ex.Log($"{nameof(CheckForConfirmRequest)}(): Conf Error={confirmResp.RootQAType.ErrorCode};");
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
            confirmReq.Accept(this.Request);
            confirmReq.EnumType = EripQAType.ConfirmRequest;
            confirmReq.RootQAType.PayRecord[0].KioskReceipt = confirmReq.RootQAType.KioskReceipt;
            confirmReq.RootQAType.PayRecord[0].ConfirmCode = "1";
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
