using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Ioc;
using Logic;
using XmlStructureComplat;

namespace WPFApp.ViewModels
{
    public class DynamicMenuWindowViewModel : BindableBase
    {
        public event Action ResponseChangedEvent = ()=>{ };
        #region ctor
        public DynamicMenuWindowViewModel()
        {
            SetCurrMenuHeaderCommand = new DelegateCommand<string>(x => LabelCurrent = x);
            SetParentGroupHeaderCommand = new DelegateCommand<string>(x => LabelParentGroup = x);
            SendParamCommand = new DelegateCommand<object>(NextPage);
            LoadedCommand = new DelegateCommand(()=> NextPage(null));
            SendVmPayrecCommand = new DelegateCommand(() => NextPage(PayrecToSend));
            SendVmAttrCommand = new DelegateCommand(() => NextPage(AttrToSend));
            LookupCommand = new DelegateCommand<Lookup>(SetLookup);
        }
        #endregion
        #region fields
        private object _selectedXmlArg;
        private string _labelGroup;
        private string _labelCurrent;
        private bool _isLoadingMenu;
        private PS_ERIP _responce;
        private PayRecord _payrecToSend;
        private List<AttrRecord> _attrToSend;
        #endregion
        #region Properties
        public PS_ERIP Responce 
        {
            get => _responce;
            set 
            { 
                SetProperty(ref _responce, value, ResponseChangedEvent);
                PayrecToSend = value?.GetListResponse?.PayRecord?.FirstOrDefault();
            }
        }
        public PayRecord PayrecToSend
        {
            get => _payrecToSend;
            set
            {
                SetProperty(ref _payrecToSend, value);
                AttrToSend = value?.AttrRecord;
            }
        }
        public List<AttrRecord> AttrToSend
        {
            get => _attrToSend;
            set => SetProperty(ref _attrToSend, value);
        }
        public object SelectedXmlArg
        {
            get => _selectedXmlArg;
            set => SetProperty(ref _selectedXmlArg, value);
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
        public bool IsLoadingMenu
        {
            get => _isLoadingMenu;
            set => SetProperty(ref _isLoadingMenu, value);
        }
        #endregion
        #region Commands Public
        public DelegateCommand<string> SetParentGroupHeaderCommand { get; }
        public DelegateCommand<string> SetCurrMenuHeaderCommand { get; }
        public DelegateCommand<object> SendParamCommand { get; }
        public DelegateCommand<Lookup> LookupCommand { get; }
        public DelegateCommand LoadedCommand { get; }
        public DelegateCommand SendVmPayrecCommand { get; }
        public DelegateCommand SendVmAttrCommand { get; }
        #endregion
        #region Command Methods
        private async void NextPage(object param)
        {
            IsLoadingMenu = !IsLoadingMenu;
            //IsLoadingMenu = true;
            Responce = await ResponceBuilder.NextPage(param);
            //IsLoadingMenu = false;
        }
        private async void SetLookup(Lookup param)
        {

        }
        #endregion
    }
}
