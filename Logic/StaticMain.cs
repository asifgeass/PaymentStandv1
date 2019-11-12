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

        public static async Task LoadSettings()
        {
            if (File.Exists(settingsPath))
            {
                try
                {
                    XDocument fileXml = await Task.Run(() => XDocument.Load(settingsPath));
                    Settings = await SerializationUtil.Deserialize<Settings>(fileXml);
                    Task.Run(async () =>
                    {
                        try
                        {
                            XDocument memory = await SerializationUtil.Serialize(Settings);
                            var inMemParts = memory.Descendants();
                            var fileParts = fileXml.Descendants();
                            if (inMemParts.Count() != fileParts.Count())
                            { SaveSettings().RunAsync(); }
                        }
                        catch (Exception ex)
                        {
                            ex.Log("Error at saving settings to file, when loading.");
                        }

                    }).RunAsync();
                }
                catch (Exception ex)
                {
                    ex.Show("Error at loading settings.");
                }
            }
            else
            {
                SaveSettings().RunAsync();
            }
        }

        public static async Task SaveSettings()
        {
            try
            {
                XDocument xml = await SerializationUtil.Serialize(Settings);
                await Task.Run(() => xml.Save(settingsPath));
            }
            catch (Exception ex)
            {
                ex.Log("Error at SaveSettings()");
            }

        }
    }
}
