using ExceptionManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Logic
{
    public static class PostGetHTTP
    {
        private static readonly string url = "http://public.softclub.by:3007/komplat/online.request";
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly Stopwatch timer = new Stopwatch();
        private static StringBuilder timings = new StringBuilder();


        public static event Action<string> WriteTextBox = (x) => { };
        public static event Action<XDocument> XmlReceived = (x) => { };
        private static bool isFirstTime = true;
        

        public static async Task<XDocument> PostStringGetXML(string destinationUrl, string requestXml)
        {
            //webClient.Proxy = WebRequest.GetSystemWebProxy();
            timings = new StringBuilder();
            try
            {
                requestXml = $"xml=<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n{requestXml}"; //standalone='yes'
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
                if(isFirstTime) request.Proxy = WebRequest.GetSystemWebProxy();
                isFirstTime = false;
                byte[] bytes;
                bytes = Encoding.UTF8.GetBytes(requestXml);
                request.ContentType = "text/xml; encoding='utf-8'";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                XDocument doc;
                Ex.Log($"=>>{requestXml}\n=>>");
                using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    WriteTextBox(requestXml);
                    requestStream.Write(bytes, 0, bytes.Length);
                }
                timer.Restart();
                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    timer.Stop();
                    timings.AppendLine($"GetResponse={timer.ElapsedMilliseconds};");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var responseStream = response.GetResponseStream())
                        {
                            var responseXml = await XmlLoadAsync(responseStream);
                            Ex.Log($"<<=\n{responseXml.ToString()}\n<<=\n{timings}");
                            doc = responseXml;
                        }
                    }
                    else
                    {
                        Ex.Log($"<<=\nError: HttpStatusCode={response.StatusCode}\n<<=\n{timings}");
                        throw new HttpRequestException($"Response isn't ok (200); code={response.StatusCode};");
                    }
                }
                WriteTextBox(doc.ToStringFull());
                return doc;
            }
            catch (Exception ex)
            {
                var msg = $"POST Error";
                WriteTextBox(msg);
                ex.Throw(msg);
                return null;
            }
        }
        public static async Task<string> PostStringGetString(string destinationUrl, string requestXml)
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
                        using (var responseStream = new StreamReader(response.GetResponseStream()))
                        {
                            string responseStr = await responseStream.ReadToEndAsync();
                            timer.Stop();
                            timings = $"Responce={timer.ElapsedMilliseconds}; {timings}\n";
                            msg = $"{timings}{responseStr}";
                        }
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
                var msg = $"1postXMLData():\n{ex.Message}";
                WriteTextBox(msg);
                throw new Exception($"POST Error", ex);
            }
        }

        public static async Task<XDocument> PostStringGetXML(string requestXml)
        {
            return await PostStringGetXML(url, $"xml=<?xml version=\"1.0\" encoding=\"UTF-8\" standalone='yes'?>\n{requestXml}");
        }
        public static async Task<string> PostStringGetString(string requestXml)
        {
            return await PostStringGetString(url, $"xml=<?xml version=\"1.0\" encoding=\"UTF-8\" standalone='yes'?>\n{requestXml}");
        }

        public static async Task<XDocument> XmlLoadAsync(Stream stream, LoadOptions loadOptions = LoadOptions.PreserveWhitespace)
        {
            return await Task.Run(() =>
            {
                XDocument doc;
                using (StreamReader sr = new StreamReader(stream))
                {
                    string str = sr.ReadToEnd().Trim();
                    str = Regex.Replace(str, @"[\p{Cc}-[\r\n\t]]+", String.Empty);
                    doc = XDocument.Parse(str, loadOptions);
                }
                return doc;
            });
        }

        public static async Task<XDocument> XmlLoadAsync(string str, LoadOptions loadOptions = LoadOptions.PreserveWhitespace)
        {
            return await Task.Run(() =>
            {
                string temp = Regex.Replace(str, @"[\p{Cc}-[\r\n\t]]+", String.Empty);
                return XDocument.Parse(temp, loadOptions);
            });
        }

        private static async Task<string> GetResponseText(string address)
        {
            return await httpClient.GetStringAsync(address);
        }
        public static string ToStringFull(this XDocument d) => $"{d.Declaration}{Environment.NewLine}{d}";

    }
}
