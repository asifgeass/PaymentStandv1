using Logic;
using Logic.XML;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPFApp
{
    public class PageVM : BindableBase
    {
        #region fields
        private PayRecord selectedPayRecord;
        private string labelGroup;
        private string labelCurrent;
        #endregion

        #region Properties
        public PS_ERIP XmlBody { get; set; }
        public object Request => ResponceBuilder.ResponseToRequest(XmlBody, XmlBody);
        public List<PayRecord> PayRecords { get; set; }
        public List<AttrRecord> AttrRecords { get; set; }
        public PayRecord SelectedPayRecord
        {
            get { return selectedPayRecord; }
            set
            {
                selectedPayRecord = value;
                RaisePropertyChanged(nameof(SelectedPayRecord));
            }
        }
        public string LabelParentGroup
        {
            get { return labelGroup; }
            set
            {
                labelGroup = value;
                RaisePropertyChanged(nameof(LabelParentGroup));
            }
        }
        public string LabelCurrent
        {
            get { return labelCurrent; }
            set
            {
                labelCurrent = value;
                RaisePropertyChanged(nameof(LabelCurrent));
            }
        }
        #endregion
    }
}
