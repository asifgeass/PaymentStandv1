using Logic.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Logic
{
    public class DynamicPage
    {
        public PS_ERIP xmlBody { get; set; }
        public PS_ERIP xmlRequest { get; set; }
        public bool IsResponced { get => xmlBody != null; }
    }
}
