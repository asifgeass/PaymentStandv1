using Logic;
using Logic.XML;
using Prism.Commands;
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
        public PageVM()
        {
            SetCurrMenuHeader = new DelegateCommand<string>(x=>LabelCurrent = x);
            SetParentGroupHeader = new DelegateCommand<string>(x => LabelParentGroup = x);
            NextPage = new DelegateCommand<object>(x=> ResponceBuilder.NextPage(x));
        }
        #region fields
        private PayRecord _selectedPayRecord;
        private string _labelGroup;
        private string _labelCurrent;
        #endregion

        #region Properties
        public PS_ERIP XmlBody { get; set; }
        public List<PayRecord> PayRecords { get; set; }
        public List<AttrRecord> AttrRecords { get; set; }
        public PayRecord SelectedPayRecord
        {
            get => _selectedPayRecord;
            set => SetProperty(ref _selectedPayRecord, value);
        }
        public string LabelParentGroup
        {
            get => _labelGroup;
            set => SetProperty(ref _labelGroup, value);
        }
        public string LabelCurrent
        {
            get => _labelCurrent;
            set => SetProperty(ref _labelCurrent, value);
        }
        #endregion

        #region Commands Public
        public DelegateCommand<string> SetParentGroupHeader { get; }
        public DelegateCommand<string> SetCurrMenuHeader { get; }
        public DelegateCommand<object> NextPage { get; }
        #endregion

        #region Methods Private
        #endregion

    }
}
