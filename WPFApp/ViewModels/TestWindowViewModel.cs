using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlStructureComplat;

namespace WPFApp.ViewModels
{
    public class TestWindowViewModel : BindableBase
    {
        private PayRecord _payrec = new PayRecord() /*{ Name = "Payrecord.Name" }*/;
        private string _str;
        private List<string> _listStr = new List<string>() { "one", "two","three"};
        public TestWindowViewModel()
        {
            payCommand = new DelegateCommand(()=>payMethod(Payrec));
            listCommand = new DelegateCommand(()=>listMethod(List));
            List<AttrRecord> attrList = _payrec.AttrRecord;
            attrList = new List<AttrRecord>() { new AttrRecord(), new AttrRecord() };
            _payrec.AttrRecord = attrList;
    }
        public PayRecord Payrec
        {
            get => _payrec;
            set => SetProperty(ref _payrec, value);
        }
        public string Str
        {
            get => _str;
            set => SetProperty(ref _str, value);
        }
        public List<string> List
        {
            get => _listStr;
            set => SetProperty(ref _listStr, value);
        }
        #region commands
        public DelegateCommand payCommand { get; }
        public DelegateCommand listCommand { get; }
        private void payMethod(PayRecord arg)
        {
            var x1 = arg.AttrRecord;
        }
        private void listMethod(List<string> arg)
        {
            var x1 = arg[1];
            var x2 = arg[2];
        }
        #endregion
    }
}
