using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XmlBodies;

namespace Logic
{
    public static class PostGetHTTP
    {        
        private static string url = "http://public.softclub.by:3007/komplat/online.request"; //"http://public.softclub.by:3007/komplat/online.request"
        private static readonly HttpClient httpClient = new HttpClient();
        public static event Action<string> WriteTextBox = (x) => { };
        //public static string IDSession { get; set; }

        public static async Task DoParalel()
        {
            postXMLData(url, XmlRequests.XmlTest().ToStringFull());
            //AnotherPost();
            //PostWithParameter();
        }
        public static async Task RunOpReq(string SessionID)
        {
            postXMLData(url, XmlRequests.XmlTest2(SessionID).ToStringFull());
        }

        public static async Task ConfirmReq()
        {
            postXMLData(url, XmlRequests.XmlTest3().ToStringFull());
        }
        public static async Task<string> AllTypePost()
        {
            var output = new StringBuilder();
            //output.AppendLine(await postXMLData(url, XmlRequests.XmlTest().ToStringFull()));
            output.AppendLine(await AnotherPost());
            //output.AppendLine(await PostWithParameter());
            return output.ToString();
        }
        public static async Task<string> GetResponseText(string address)
        {
            return await httpClient.GetStringAsync(address);
        }
        public static async Task<string> postXMLData(string destinationUrl, string requestXml)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);                
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(requestXml);
                request.ContentType = "text/xml; encoding='utf-8'";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
                string msg;
                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var responseStream = new StreamReader(response.GetResponseStream()))
                        {
                            string responseStr = await responseStream.ReadToEndAsync();
                            msg = $"1postXMLData():\n{responseStr}";
                        }
                    }
                    else
                    {
                        msg = $"1postXMLData():\nresponse.StatusCode={response.StatusCode}\n{response.StatusDescription}";

                    }
                }
                WriteTextBox(msg);
                return msg;
            }
            catch (Exception ex)
            {
                var msg= $"1postXMLData():\n{ex.Message}";
                WriteTextBox(msg);
                return msg;
            }

        }
        public static async Task<string> AnotherPost()
        {
            try
            {
                string xmlMessage = /*"XML=" +*/ XmlRequests.XmlTest().ToStringFull();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                byte[] requestInFormOfBytes = System.Text.Encoding.ASCII.GetBytes(xmlMessage);
                request.Method = "POST";
                request.ContentType = "text/xml;charset=utf-8";
                request.ContentLength = requestInFormOfBytes.Length;

                using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    await requestStream.WriteAsync(requestInFormOfBytes, 0, requestInFormOfBytes.Length);                    
                }
                string msg;
                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader respStream = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8))
                        {
                            string receivedResponse = await respStream.ReadToEndAsync();
                            msg = $"2AnotherPost():\n{receivedResponse}";
                        }
                    }
                    else
                    {
                        msg= $"2AnotherPost():\nresponse.StatusCode={response.StatusCode}\n{response.StatusDescription}";
                    }
                }
                WriteTextBox(msg);
                return msg;
            }
            catch (Exception ex)
            {
                var msg= $"2AnotherPost():\n{ex.Message}";
                WriteTextBox(msg);
                return msg;
            }
        }
        public static async Task<string> PostWithParameter()
        {
            try
            {
                var xmlRequest = XmlRequests.XmlTest();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                
                // parameters to post - other end expects API and XML parameters
                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("XML", xmlRequest.ToStringFull()));

                // assemble the request content form encoded (reference System.Net.Http)
                HttpContent content = new FormUrlEncodedContent(postData);

                // indicate what we are posting in the request
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = content.Headers.ContentLength.Value;
                await content.CopyToAsync(await request.GetRequestStreamAsync());
                string msg;
                // get response
                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        // as an xml: deserialise into your own object or parse as you wish
                        var responseXml = XDocument.Load(response.GetResponseStream());
                        msg= $"3PostWithParameter():\n{responseXml.ToString()}";
                    }
                    else
                    {
                        msg= $"3PostWithParameter():\nresponse.StatusCode={response.StatusCode}\n{response.StatusDescription}";
                    }

                }
                WriteTextBox(msg);
                return msg;
            }
            catch (Exception ex)
            {                
                var msg= $"3PostWithParameter():\n{ex.Message}";
                WriteTextBox(msg);
                return msg;
            }
        }

    }
}
