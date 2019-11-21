﻿using System;
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
using ExceptionManager;
using XmlStructureComplat.Validators;
using FluentValidation;
using System.ComponentModel;
using System.Collections;
using FluentValidation.Results;

namespace WPFApp.ViewModels
{
    public class DynamicMenuWindowViewModel : ValidatableBindableBase /*ValidatableBindableBase*/ //, INotifyDataErrorInfo
    {
        public event Action NewResponseComeEvent = ()=>{ };
        public event Action PaymentWaitingEvent = () => { };
        private PayRecordValidator payValidator = new PayRecordValidator();

        #region ctor
        public DynamicMenuWindowViewModel()
        {
            HomePageCommand = new DelegateCommand(HomePage).ObservesCanExecute(() => IsHomeButtonActive);
            BackUserCommand = new DelegateCommand(BackPage).ObservesCanExecute(() => IsBackButtonActive);
            SetCurrMenuHeaderCommand = new DelegateCommand<string>(x => LabelCurrent = x);
            SetParentGroupHeaderCommand = new DelegateCommand<string>(x => LabelParentGroup = x);
            SendParamCommand = new DelegateCommand<object>(async x=>await NextPage(x));
            NextPageCommand = new DelegateCommand(async() => await NextPage(null));
            NextPageTestValidate = new DelegateCommand(async() => await NextPageValidate(null));
            //NewResponseComeEvent += ClearLookup;
        }

        #endregion

        #region fields
        private object _selectedXmlArg;
        private string _labelGroup;
        private string _labelCurrent;
        private bool _isLoadingMenu;
        private PS_ERIP _responce;
        private PayRecord _payrecToSend;
        private PS_ERIP _eripToSend = new PS_ERIP();
        private Exception _exception;
        private bool _isHomeButtonActive = true;
        private bool _isBackButtonActive;
        #endregion

        #region Properties
        public PS_ERIP Responce 
        {
            get => _responce;
            set
            {                
                SetProperty(ref _responce, value);
                DoOnResponseCome();
                NewResponseComeEvent();
            }
        }

        private void DoOnResponseCome()
        {
            IsCustomLoadingScreen = false;
            Task.Run(async () =>
            {
                var xml = await SerializationUtil.Serialize(Responce);
                Ex.Log($"Response came to VIEW\n{xml.ToString()}\nResponse came to VIEW\n");
            });
            if (Responce?.ResponseReq?.PayRecord?.Count == 1)
            { PayrecToSend = Responce?.ResponseReq?.PayRecord?.FirstOrDefault(); }
            EripToSend = new PS_ERIP();
            this.IsHomeButtonActive = true;
            IsBackButtonActive = IsBackRequestPossible;
        }

        public PayRecord PayrecToSend
        {
            get => _payrecToSend;
            set
            {
                SetProperty(ref _payrecToSend, value);
            }
        }
        public string PayrecToSendSumma
        {
            get => PayrecToSend?.Summa;
            set
            {
                Ex.Log($"mainVM: PayrecToSendSumma SETTER val={value};");
                if (PayrecToSend == null) PayrecToSend = new PayRecord();
                PayrecToSend.Summa = value;                
                Ex.TryLog(() =>
                {
                    var valResult = payValidator.Validate(_payrecToSend);
                    var isValid = ValidateResult(valResult);
                });
                RaisePropertyChanged();
            }
        }
        public PS_ERIP EripToSend
        {
            get => _eripToSend;
            set
            {
                SetProperty(ref _eripToSend, value);
            }
        }
        public List<AttrValidationVM> AttrVMList { get; private set; } = new List<AttrValidationVM>();
        public List<LookupVM> LookupVMList { get; private set; } = new List<LookupVM>();
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
        public bool IsCustomLoadingScreen { get; set; }
        public bool IsHomeButtonActive
        {
            get => _isHomeButtonActive;
            set => SetProperty(ref _isHomeButtonActive, value);
        }
        public bool IsBackButtonActive
        {
            get => _isBackButtonActive;
            set => SetProperty(ref _isBackButtonActive, value);
        }
        public bool IsFinalButtonActive { get; set; }
        public bool IsAttrValid { get; set; }
        public bool IsPayrecValid { get; set; }
        public bool IsBackRequestPossible => StaticMain.IsBackPossible();
        public Exception Exception
        {
            get => _exception;
            set 
            {
                ///если мы хотим вернуться на предыдущую страницу (которая по факту
                ///может остаться текущей, ибо ошибка может случится ДО получения респонса)
                ///если респонс получает то же значение что уже имеет, то не срабатывает 
                ///ивент получения респонса и поэтмоу View не поменяется
                //_responce = null; 
                SetProperty(ref _exception, value);
                this.IsHomeButtonActive = true;
                Ex.TryLog(()=> IsBackButtonActive = IsBackRequestPossible);
            }
        }
        #endregion

        #region Public Methods
        public LookupVM GetNewLookupVM()
        {
            var vm = new LookupVM();
            LookupVMList.Add(vm);
            return vm;
        }
        public AttrValidationVM GetNewAttrVM(AttrRecord AttrRecord)
        {
            var vm = new AttrValidationVM(AttrRecord);
            AttrVMList.Add(vm);
            return vm;
        }
        #endregion

        #region Public Commands
        public DelegateCommand HomePageCommand { get; }
        public DelegateCommand BackUserCommand { get; }
        public DelegateCommand<string> SetParentGroupHeaderCommand { get; }
        public DelegateCommand<string> SetCurrMenuHeaderCommand { get; }
        public DelegateCommand<object> SendParamCommand { get; }
        public DelegateCommand<LookupItem> LookupCommand { get; }
        public DelegateCommand NextPageCommand { get; }
        public DelegateCommand NextPageTestValidate { get; }
        #endregion

        #region Pages
        private async Task NextPage(object param=null)
        {
            try
            {
                this.IsHomeButtonActive = false;
                this.IsBackButtonActive = false;
                if (!IsCustomLoadingScreen) IsLoadingMenu = !IsLoadingMenu;
                Ex.Log($"VM => Logic NextPage() param={param};");
                FillWithLookupsVM();
                FillWithAttrInputVM();
                Ex.Try(() => PayrecToSend.Summa = PayrecToSend.Summa.Replace(",","."));
                EripToSend.ResponseReq.PayRecord = new List<PayRecord>() { PayrecToSend };
                Responce = await StaticMain.NextPage(param ?? EripToSend);
            }
            catch (Exception ex)
            {
                Exception = ex;
            }
        }
        private async Task NextPageValidate(object param = null)
        {
            Ex.Log($"VM: {nameof(NextPageValidate)}(): validate sum={PayrecToSend.Summa};");
            bool isValid = true;
            Ex.TryLog(() =>
            {
                var valResult = payValidator.Validate(PayrecToSend);
                isValid = ValidateResult(valResult, nameof(PayrecToSend.Summa));                
            });
            if (isValid)
            {
                IsCustomLoadingScreen = true;
                PaymentWaitingEvent();
                await NextPage(param);
            }
        }

        private async void BackPage()
        {
            try
            {
                this.IsBackButtonActive = false;
                IsLoadingMenu = !IsLoadingMenu;
                Ex.Log($"VM => Logic BackPage()");
                Responce = await StaticMain.BackPage();
            }
            catch (Exception ex)
            {
                Exception = ex;
            }
        }

        private async void HomePage()
        {
            try
            {
                this.IsHomeButtonActive = false;
                IsLoadingMenu = !IsLoadingMenu;
                Ex.Log($"VM => Logic BackPage()");
                Responce = await StaticMain.HomePage();
            }
            catch (Exception ex)
            {
                Exception = ex;
            }
        }
        #endregion

        #region Private Methods
        private void FillWithLookupsVM()
        {
            Ex.Log($"{nameof(FillWithLookupsVM)}(): childVM count={LookupVMList.Count}");
            foreach (var lookupVM in LookupVMList)
            {
                var selectedValue = lookupVM?.Lookup?.SelectedItem?.Value;
                Ex.Log($"{nameof(FillWithLookupsVM)}(): lookup: name={lookupVM.Lookup.Name}; selected value={selectedValue}");
                foreach (var attr in PayrecToSend?.AttrRecord)
                {
                    if (attr?.Lookup == lookupVM?.Lookup?.Name && attr?.Lookup != null)
                    {
                        Ex.Log($"{nameof(FillWithLookupsVM)}(): Match! attr: name={attr.Name}; value={attr.Value}; newValue={selectedValue}");
                        attr.Value = selectedValue;
                    }
                }
            }
        }
        private void FillWithAttrInputVM()
        {
            Ex.Log($"{nameof(FillWithAttrInputVM)}(): childVM count={AttrVMList.Count}");
            foreach (var vmAttr in AttrVMList)
            {
                var nameAttr = vmAttr?.AttrRecord?.Name;
                foreach (var attr in PayrecToSend?.AttrRecord)
                {
                    if (attr?.Name == nameAttr && attr?.Name!=null)
                    {
                        Ex.Log($"{nameof(FillWithAttrInputVM)}(): Match! attr: name={attr.Name}; value={attr.Value}; newValue={vmAttr.ValueAttrRecord}");
                        attr.Value = vmAttr.ValueAttrRecord;
                    }
                }
            }
        }
        //private void ClearLookup()
        //{
        //    _lookupVMList = new List<LookupVM>();
        //    _lookupVM = null; ;
        //}
        #endregion

        //#region iNotifyDataErrorInfo 2019 Matyaszek
        //private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();
        //public bool HasErrors => _errorsByPropertyName.Any();
        //public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        //public IEnumerable GetErrors(string propertyName)
        //{
        //    return (_errorsByPropertyName.ContainsKey(propertyName)) ?
        //        _errorsByPropertyName[propertyName] : null;
        //}
        //private void OnErrorsChanged(string propertyName)
        //{
        //    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        //}
        //private void AddError(string propertyName, string error)
        //{
        //    if (!_errorsByPropertyName.ContainsKey(propertyName))
        //        _errorsByPropertyName[propertyName] = new List<string>();

        //    if (!_errorsByPropertyName[propertyName].Contains(error))
        //    {
        //        _errorsByPropertyName[propertyName].Add(error);                
        //        OnErrorsChanged(propertyName);
        //    }
        //}
        //private void ClearErrors(string propertyName)
        //{
        //    if (_errorsByPropertyName.ContainsKey(propertyName))
        //    {
        //        _errorsByPropertyName.Remove(propertyName);
        //        OnErrorsChanged(propertyName);
        //    }
        //}
        //private void ValidatePropertyExample(string UserName)
        //{
        //    ClearErrors(nameof(UserName));
        //    if (string.IsNullOrWhiteSpace(UserName))
        //    { AddError(nameof(UserName), "Username cannot be empty."); }
        //    if (string.Equals(UserName, "Admin", StringComparison.OrdinalIgnoreCase))
        //    { AddError(nameof(UserName), "Admin is not valid username."); }
        //    if (UserName == null || UserName?.Length <= 5)
        //    { AddError(nameof(UserName), "Username must be at least 6 characters long."); }
        //}
        //#endregion
    }
}
