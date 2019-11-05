using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XmlStructureComplat;

namespace Logic
{
    public class VMToLogicStatic
    {
        //private static Lazy<PagesManager> manager = new Lazy<PagesManager>();
        private static XmlTransactionsManager manager = new XmlTransactionsManager();
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
            return manager.IsPrevRequestPossible();
        }

        public static Task<PS_ERIP> HomePage()
        {
            return manager.HomeRequest();
        }
    }
}
