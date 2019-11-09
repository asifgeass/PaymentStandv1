using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XmlStructureComplat;
using ExceptionManager;
using System.IO;

namespace Logic
{
    public class StaticMain
    {
        //private static Lazy<PagesManager> manager = new Lazy<PagesManager>();
        private static XmlTransactionsManager manager = new XmlTransactionsManager();      
        private static string settingsPath = $@"{Environment.CurrentDirectory}\settings.xml";
        private static readonly Settings defaultSettings = new Settings()
        {
            //ERIP = new XML.EripComplatSettings()
            //{
            //    TerminalID = "TEST_TERMINAL",
            //    version = "3",
            //    url = @"http://public.softclub.by:3007/komplat/online.request"
            //},
            //Terminal_MdomPOS = new XML.TerminalSettings()
            //{  
            //    TerminalID = "KKM1", 
            //    version = "1", 
            //    url = @"http://public.softclub.by:8010/mdom_pos/online.request"
            //}
        };
        public static Settings Settings { get; private set; } = new Settings();
        public static async Task<PS_ERIP> NextPage(object select)
        {
            return await manager.NextRequest(select);
        }

        public static async Task<PS_ERIP> BackPage()
        {
            return await manager.PrevRequest();
        }

        public static bool IsBackPossible()
        {
            try
            {
                return manager.IsPrevRequestPossible();
            }
            catch (Exception ex)
            {
                ex.Log();
                return true;
            }

        }

        public static Task<PS_ERIP> HomePage()
        {
            LoadSettings().RunAsync();
            return manager.HomeRequest();
        }
        public static async Task<Settings> LoadSettings()
        {
            XDocument xml;
            if (File.Exists(settingsPath))
            {
                xml = await Task.Run(()=> XDocument.Load(settingsPath));                
                Settings = await SerializationUtil.Deserialize<Settings>(xml);
                Task.Run(async() =>
                {
                    var memory = await SerializationUtil.Serialize(Settings);
                    if(!xml.Equals(memory))
                    { SaveSettings().RunAsync(); }
                }).RunAsync();
                
            }
            else
            {
                SaveSettings().RunAsync();
            }
            return Settings;
        }

        public static async Task SaveSettings()
        {
            XDocument xml = await SerializationUtil.Serialize(Settings);
            await Task.Run(()=> xml.Save(settingsPath));
        }
    }
}
