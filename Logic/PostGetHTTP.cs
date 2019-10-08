using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private static readonly string url = "http://public.softclub.by:3007/komplat/online.request"; //"http://public.softclub.by:3007/komplat/online.request"
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly Stopwatch timer = new Stopwatch();

        public static event Action<string> WriteTextBox = (x) => { };
        public static event Action<XDocument> XmlReceived = (x) => { };

        static PostGetHTTP()
        {
            XmlReceived += ResponceBuilder.PostGetHTTP_XmlReceived;
        }
        //public static string IDSession { get; set; }

        public static async Task<XDocument> XmlLoadAsync(Stream stream
            , LoadOptions loadOptions = LoadOptions.PreserveWhitespace)
        {
            return await Task.Run(() =>
            {
                return XDocument.Load(stream, loadOptions);
            });
        }

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

        private static async Task<string> GetResponseText(string address)
        {
            return await httpClient.GetStringAsync(address);
        }
        private static async Task<string> postXMLData(string destinationUrl, string requestXml)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);                
                byte[] bytes;
                bytes = System.Text.Encoding.UTF8.GetBytes(requestXml);
                request.ContentType = "text/xml; encoding='utf-8'";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                string timings = "";
                string msg;
                timer.Restart();
                using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    timer.Stop();
                    timings += $"GetRequestStream={timer.ElapsedMilliseconds}; ";
                    WriteTextBox(requestXml);
                    timer.Restart();
                    requestStream.Write(bytes, 0, bytes.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var responseStream = response.GetResponseStream())
                        {
                            var responseXml = await XmlLoadAsync(responseStream);
                            timer.Stop();
                            XmlReceived(responseXml);
                            timings = $"Responce={timer.ElapsedMilliseconds}; {timings}\n";
                            msg = $"{timings}{responseXml.ToStringFull()}";
                        }
                        //using (var responseStream = new StreamReader(response.GetResponseStream()))
                        //{
                        //    string responseStr = await responseStream.ReadToEndAsync();
                        //    timer.Stop();
                        //    timings = $"Responce={timer.ElapsedMilliseconds}; {timings}\n";
                        //    msg = $"{timings}{responseStr}";
                        //}
                    }
                    else
                    {
                        timer.Stop();
                        timings = $"Responce={timer.ElapsedMilliseconds}; {timings}\n";
                        msg = $"{timings}response.StatusCode={response.StatusCode}\n{response.StatusDescription}";

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

        public static async Task<string> postXMLData(string requestXml)
        {
            return await postXMLData(url, $"<?xml version=\"1.0\" encoding=\"UTF-8\" standalone='yes'?>\n{requestXml}");
        }
    }
}
