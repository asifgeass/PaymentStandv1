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
using System.Diagnostics;

namespace WPFApp.ViewModels
{
    public class DynamicMenuWindowViewModel : BindableBase
    {
        public event Action NewResponseComeEvent = ()=>{ };

        #region ctor
        public DynamicMenuWindowViewModel()
        {
            //SendVmAttrCommand = new DelegateCommand(() => NextPage(AttrToSend));
            SetCurrMenuHeaderCommand = new DelegateCommand<string>(x => LabelCurrent = x);
            SetParentGroupHeaderCommand = new DelegateCommand<string>(x => LabelParentGroup = x);
            SendParamCommand = new DelegateCommand<object>(NextPage);
            LoadedCommand = new DelegateCommand(()=> NextPage(null));
            SendVmPayrecCommand = new DelegateCommand(() => NextPage(PayrecToSend));
            LookupCommand = new DelegateCommand<LookupItem>(SetLookup);
            HomePageCommand = new DelegateCommand( () => { } );
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
        private Exception _exception;
        #endregion

        #region Properties
        public PS_ERIP Responce 
        {
            get => _responce;
            set 
            {
                if (value?.GetListResponse?.PayRecord?.Count == 1)
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
                //Trace.WriteLine($"{nameof(PayrecToSend)}():");
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
        public void ClearChildLookupVM()
        {
            _lookupVMList = new List<LookupVM>();
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
        public Exception Exception
        {
            get => _exception;
            set => SetProperty(ref _exception, value);
        }
        #endregion

        #region Public Commands
        public DelegateCommand<string> SetParentGroupHeaderCommand { get; }
        public DelegateCommand<string> SetCurrMenuHeaderCommand { get; }
        public DelegateCommand<object> SendParamCommand { get; }
        public DelegateCommand<LookupItem> LookupCommand { get; }
        public DelegateCommand LoadedCommand { get; }
        public DelegateCommand HomePageCommand { get; }
        public DelegateCommand BackUserCommand { get; }
        public DelegateCommand SendVmPayrecCommand { get; }
        //public DelegateCommand SendVmAttrCommand { get; }
        #endregion

        #region Private Methods
        private async void NextPage(object param=null)
        {
            try
            {
                IsLoadingMenu = !IsLoadingMenu;
                FillPayrecToSendWithLookup();
                Trace.WriteLine($"VM => Logic NextPage() param={param}; PayrecToSend={PayrecToSend?.Name}");
                Responce = await ResponceBuilder.NextPage(param ?? PayrecToSend);
            }
            catch (Exception ex)
            {
                Exception = ex;
            }
        }

        private async void BackPage()
        {
            try
            {
                IsLoadingMenu = !IsLoadingMenu;
                Trace.WriteLine($"VM => Logic BackPage()");
                Responce = await ResponceBuilder.BackPage();
            }
            catch (Exception ex)
            {
                Exception = ex;
            }
        }

        private void FillPayrecToSendWithLookup()
        {
            Trace.WriteLine($"{nameof(FillPayrecToSendWithLookup)}(): childVM count={ChildLookupVMList.Count}");
            foreach (var lookupVM in ChildLookupVMList)
            {
                var selectedValue = lookupVM?.Lookup?.SelectedItem?.Value;
                Trace.WriteLine($"{nameof(FillPayrecToSendWithLookup)}(): lookup: name={lookupVM.Lookup.Name}; selected value={selectedValue}");
                foreach (var attr in PayrecToSend?.AttrRecord)
                {
                    if (attr?.Lookup == lookupVM?.Lookup?.Name && attr?.Lookup!=null)
                    {
                        Trace.WriteLine($"{nameof(FillPayrecToSendWithLookup)}(): Match! attr: name={attr.Name}; value={attr.Value}; newValue={selectedValue}");
                        attr.Value = selectedValue;
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
