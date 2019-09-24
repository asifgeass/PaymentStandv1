using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Logic
{
    public class PostGetHTTP
    {        
        private static string url = "http://komplat/online.request"; //"http://82.209.232.62:8080/komplat/online.request"82.209.232.62
        public static string Main()
        {
            var r1 = postXMLData(url, GetPayList().ToString());
            var r2 = AnotherPost();
            var r3 = PostWithParameter();
            return $"{r1}\n{r2}\n{r3}";
        }
        public static string postXMLData(string destinationUrl, string requestXml)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(requestXml);
                request.ContentType = "text/xml; encoding='utf-8'";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    string responseStr = new StreamReader(responseStream).ReadToEnd();
                    return responseStr;
                }
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public static string AnotherPost()
        {
            try
            {
                string xmlMessage = "XML=<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n" + GetPayList().ToString();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                byte[] requestInFormOfBytes = System.Text.Encoding.ASCII.GetBytes(xmlMessage);
                request.Method = "POST";
                request.ContentType = "text/xml;charset=utf-8";
                request.ContentLength = requestInFormOfBytes.Length;
                
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(requestInFormOfBytes, 0, requestInFormOfBytes.Length);
                requestStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader respStream = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);
                string receivedResponse = respStream.ReadToEnd();
                respStream.Close();
                response.Close();
                return receivedResponse;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public static string PostWithParameter()
        {
            try
            {
                var xmlRequest = GetPayList();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                
                // parameters to post - other end expects API and XML parameters
                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("XML", xmlRequest.ToString()));

                // assemble the request content form encoded (reference System.Net.Http)
                HttpContent content = new FormUrlEncodedContent(postData);

                // indicate what we are posting in the request
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = content.Headers.ContentLength.Value;
                content.CopyToAsync(request.GetRequestStream()).Wait();

                // get response
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // as an xml: deserialise into your own object or parse as you wish
                    var responseXml = XDocument.Load(response.GetResponseStream());
                    return responseXml.ToString();
                }
                return null;
            }
            catch (Exception ex)
            {                
                return ex.Message;
            }
        }

        public static XElement RunOpRequestGosTehOsmotr()
        {
            XElement xml = new XElement("PS_ERIP",
              new XElement("RunOperationRequest",
                new XElement("Key"),                
                new XElement("TerminalID", "TEST_TERMINAL"),
                new XElement("Version", "3"),
                new XElement("SignCode", "0"),
                new XElement("PayDate", "17/09/2019 09:00:06"),
                new XElement("KioskReceipt", "17092019090006"),
                new XElement("KioskError", "0"),
                new XElement("PayRecordCount", "0"),
                new XElement("PayRecord",
                    new XAttribute("Code", "421"),
                    new XAttribute("DIType", "9120"),
                    new XAttribute("Name", "Гостехосмотр"),
                    new XAttribute("CodeOut", "421"),
                    new XAttribute("StornoMode", "S"),
                    new XAttribute("ClaimID", "0"),
                    new XAttribute("Category", "1"),
                    new XAttribute("GetPayListType", "1"),
                    new XAttribute("Summa", "8"),
                    new XAttribute("Edit", "0"),
                    new XAttribute("View", "0"),
                    new XAttribute("Fine", "0"),
                    new XAttribute("PayCommission", "0"),
                    new XAttribute("Commission", "0")
                    ),
                new XElement("GroupRecord",
                    new XAttribute("Code", "401"),
                    new XAttribute("Name", "Физическое лицо")
                    ),
                new XElement("AttrRecord",
                    new XAttribute("Code", "660"),
                    new XAttribute("CodeOut", "1001"),
                    new XAttribute("Name", "Оплачено по услуге"),
                    new XAttribute("Value", "1234AA-7")
                    ),
                new XElement("AttrRecord",
                    new XAttribute("Code", "745"),
                    new XAttribute("CodeOut", "1002"),
                    new XAttribute("Name", "Свидетельство о регистрации"),
                    new XAttribute("Value", "AAA123456789")
                    ),
                new XElement("AttrRecord",
                    new XAttribute("Code", "873"),
                    new XAttribute("Name", "УНП плательщика"),
                    new XAttribute("Value", "109879310")
                    ),
                new XElement("AttrRecord",
                    new XAttribute("Code", "661"),
                    new XAttribute("CodeOut", "1002"),
                    new XAttribute("Name", "Тип транспортного средства"),
                    new XAttribute("Value", "Легковой автомобиль")
                    ),
                new XElement("AttrRecord",
                    new XAttribute("Code", "664"),
                    new XAttribute("Name", "Тип двигателя"),
                    new XAttribute("Value", "Гибридный или электромобиль")
                    ),
                new XElement("AttrRecord",
                    new XAttribute("Code", "665"),
                    new XAttribute("Name", "Колесная формула"),
                    new XAttribute("Value", "4x2")
                    ),
                new XElement("AttrRecord",
                    new XAttribute("Code", "861"),
                    new XAttribute("Name", "Счетчик запросов"),
                    new XAttribute("Value", "0")
                    )));            
            return xml;
        }
        public static XElement GetPayList()
        {
            XElement xml = new XElement
                ("PS_ERIP",
                    new XElement("GetPayListRequest",                
                        new XElement("TerminalID", "TEST_TERMINAL"),
                        new XElement("Version", "3"),
                        new XElement("PayCode", "400")
                    )
                );
            return xml;
        }
    }
}
