using Logic.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Logic
{
    public class ResponceBuilder
    {
        //private static Lazy<PagesManager> manager = new Lazy<PagesManager>();
        private static PagesManager manager = new PagesManager();
        public static async Task<PS_ERIP> NextPage(object select)
        {
            return await manager.NextRequest();
        }
    }
}
