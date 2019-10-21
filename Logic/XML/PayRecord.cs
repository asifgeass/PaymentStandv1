using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.XML
{
    public class PayRecord
    {
        private GroupRecord GroupRecord { get; set; }
        private List<Lookup> Lookups { get; set; }
        private List<AttrRecord> AttrRecords { get; set; }
        private string Code { get; set; }
        private string Name { get; set; }
        private string StornoMode { get; set; }
        private string Commission { get; set; }
        private string Summa { get; set; }

        private string View { get; set; }
        private string Fine { get; set; }
        private string PayCommission { get; set; }
        private string SessionId { get; set; }

        private string Edit { get; set; }
        private string GetPayListType { get; set; }
        private string RecordID { get; set; }
        private string PaymentID { get; set; }
        private string xx { get; set; }
    }
}
