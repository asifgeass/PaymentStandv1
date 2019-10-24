using Logic.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models
{
    public class PageModel
    {
        public List<PayRecord> PayRecords { get; set; }
        public PayRecord SelectedPayRecord { get; set; } = null;
    }
}
