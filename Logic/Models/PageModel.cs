using Logic.XML;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models
{
    public class PageModel : INotifyPropertyChanged
    {
        private PayRecord selectedPayRecord;
        public event PropertyChangedEventHandler PropertyChanged;
        public List<PayRecord> PayRecords { get; set; }
        public PayRecord SelectedPayRecord
        {
            get { return selectedPayRecord; }
            set
            {
                selectedPayRecord = value;
                OnPropertyChanged(nameof(SelectedPayRecord));
            }
        }
        
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
