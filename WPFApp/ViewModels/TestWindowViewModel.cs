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
        private PayRecord _test = new PayRecord() /*{ Name = "Payrecord.Name" }*/;
        private string _str = "test";
        private List<string> _listStr = new List<string>() { "one", "two","three"};
        public PayRecord Test
        {
            get => _test;
            set => SetProperty(ref _test, value);
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
    }
}
