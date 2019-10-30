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
        public event Action NewResponseComeEvent = ()=>{ };

        #region ctor
        public DynamicMenuWindowViewModel()
        {
            SetCurrMenuHeaderCommand = new DelegateCommand<string>(x => LabelCurrent = x);
            SetParentGroupHeaderCommand = new DelegateCommand<string>(x => LabelParentGroup = x);
            SendParamCommand = new DelegateCommand<object>(NextPage);
            LoadedCommand = new DelegateCommand(()=> NextPage(null));
            SendVmPayrecCommand = new DelegateCommand(() => NextPage(PayrecToSend));
            SendVmAttrCommand = new DelegateCommand(() => NextPage(AttrToSend));
            LookupCommand = new DelegateCommand<LookupItem>(SetLookup);
            NewResponseComeEvent += ()=>ClearLookup();
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
        private LookupVM _lookupVM;
        private List<LookupVM> _lookupVMList = new List<LookupVM>();
        #endregion

        #region Properties
        public PS_ERIP Responce 
        {
            get => _responce;
            set 
            {
                if (value.GetListResponse.PayRecord.Count == 1)
                { PayrecToSend = value?.GetListResponse?.PayRecord?.FirstOrDefault(); }
                SetProperty(ref _responce, value, NewResponseComeEvent);
            }
        }
        public PayRecord PayrecToSend
        {
            get => _payrecToSend;
            set
            {
                AttrToSend = value?.AttrRecord;
                SetProperty(ref _payrecToSend, value);
            }
        }
        public List<AttrRecord> AttrToSend
        {
            get => _attrToSend;
            set => SetProperty(ref _attrToSend, value);
        }
        public LookupVM GetNewLookupVM()
        {
            var vm = new LookupVM();
            _lookupVMList.Add(vm);
            return vm;
        }
        public List<LookupVM> ChildLookupVMList => _lookupVMList;
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

        #region Public Commands
        public DelegateCommand<string> SetParentGroupHeaderCommand { get; }
        public DelegateCommand<string> SetCurrMenuHeaderCommand { get; }
        public DelegateCommand<object> SendParamCommand { get; }
        public DelegateCommand<LookupItem> LookupCommand { get; }
        public DelegateCommand LoadedCommand { get; }
        public DelegateCommand SendVmPayrecCommand { get; }
        public DelegateCommand SendVmAttrCommand { get; }
        #endregion

        #region Private Methods
        private async void NextPage(object param)
        {
            IsLoadingMenu = !IsLoadingMenu;
            //IsLoadingMenu = true;
            //LookupVMList.ForEach(item => item.Lookup.SelectedItem.Value);
            FillPayrecToSendWithLookup();
            Responce = await ResponceBuilder.NextPage(param ?? PayrecToSend);
            //IsLoadingMenu = false;
        }

        private void FillPayrecToSendWithLookup()
        {
            foreach (var lookupVM in ChildLookupVMList)
            {
                foreach (var attr in PayrecToSend?.AttrRecord)
                {
                    if (attr?.Lookup == lookupVM?.Lookup?.Name)
                    {
                        attr.Value = lookupVM?.Lookup?.SelectedItem?.Value;
                    }
                }
            }
        }

        private async void ClearLookup()
        {
            _lookupVMList = new List<LookupVM>();
            _lookupVM = null; ;
        }
        private async void SetLookup(LookupItem param)
        {
            
        }

        #endregion
    }
}
