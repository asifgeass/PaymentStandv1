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
using ExceptionManager;
using XmlStructureComplat.Validators;
using FluentValidation;
using System.ComponentModel;
using System.Collections;
using FluentValidation.Results;

namespace WPFApp.ViewModels
{
    public class DynamicMenuWindowViewModel : BindableBase /*ValidatableBindableBase*/ , INotifyDataErrorInfo
    {
        public event Action NewResponseComeEvent = ()=>{ };
        private readonly PayRecordValidator payValidator = new PayRecordValidator();
        private readonly AttrRecordValidator attrValidator = new AttrRecordValidator();

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
            NewResponseComeEvent += ClearLookup;
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
        private List<AttrRecord> _attrToSend;
        private LookupVM _lookupVM;
        private List<LookupVM> _lookupVMList = new List<LookupVM>();
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
                this.IsHomeButtonActive = true;
                SetProperty(ref _responce, value);                
                Task.Run(async () =>
                {
                    var xml = await SerializationUtil.Serialize(value);
                    Ex.Log($"Response came to VIEW\n{xml.ToString()}\nResponse came to VIEW\n");
                });
                if (Responce?.ResponseReq?.PayRecord?.Count == 1)
                { PayrecToSend = Responce?.ResponseReq?.PayRecord?.FirstOrDefault(); }
                EripToSend = new PS_ERIP();
                IsBackButtonActive = IsBackRequestPossible;
                NewResponseComeEvent();
            }
        }
        public PayRecord PayrecToSend
        {
            get => _payrecToSend;
            set
            {
                Ex.Log($"VM: PayrecToSend changed: new sum={value?.Summa};");
                SetProperty(ref _payrecToSend, value);
                var valResult = payValidator.Validate(_payrecToSend);
                var isValid = ValidateResult(valResult, nameof(_payrecToSend.Summa));
                AttrToSend = value?.AttrRecord;
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
                IsLoadingMenu = !IsLoadingMenu;
                Ex.Log($"VM => Logic NextPage() param={param};");
                FillPayrecToSendWithLookup();
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
            var valResult = payValidator.Validate(PayrecToSend);
            var isValid = ValidateResult(valResult, nameof(PayrecToSend.Summa));
            if (isValid) { await NextPage(param); }
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
        private void FillPayrecToSendWithLookup()
        {
            Ex.Log($"{nameof(FillPayrecToSendWithLookup)}(): childVM count={ChildLookupVMList.Count}");
            foreach (var lookupVM in ChildLookupVMList)
            {
                var selectedValue = lookupVM?.Lookup?.SelectedItem?.Value;
                Ex.Log($"{nameof(FillPayrecToSendWithLookup)}(): lookup: name={lookupVM.Lookup.Name}; selected value={selectedValue}");
                foreach (var attr in PayrecToSend?.AttrRecord)
                {
                    if (attr?.Lookup == lookupVM?.Lookup?.Name && attr?.Lookup!=null)
                    {
                        Ex.Log($"{nameof(FillPayrecToSendWithLookup)}(): Match! attr: name={attr.Name}; value={attr.Value}; newValue={selectedValue}");
                        attr.Value = selectedValue;
                    }
                }
            }
        }

        private void ClearLookup()
        {
            _lookupVMList = new List<LookupVM>();
            _lookupVM = null; ;
        }
        #endregion

        #region INotifyDataErrorInfo members 2013 Magnus Montin
        private bool ValidateResult(ValidationResult valResult, string nameArg)
        {
            if (valResult == null) return true;
            var propertyKey = nameArg;
            
            if (!valResult.IsValid)
            {
                _validationErrors[propertyKey] = valResult.Errors;
                /* Raise event to tell WPF to execute the GetErrors method */
                RaiseErrorsChanged(propertyKey);
            }
            else if (_validationErrors.ContainsKey(propertyKey))
            {
                /* Remove all errors for this property */
                _validationErrors.Remove(propertyKey);
                /* Raise event to tell WPF to execute the GetErrors method */
                RaiseErrorsChanged(propertyKey);
            }
            Ex.Log($"INotifyDataErrorInfo: ValidateResult({valResult}, {nameArg})");
            return valResult.IsValid;
        }
        //private async void ValidatePropertyExample(string username)
        //{
        //    const string propertyKey = "Username";
        //    ICollection<string> validationErrors = null;
        //    /* Call service asynchronously */
        //    bool isValid = await Task<bool>.Run(() =>
        //    {
        //        var valResult = payValidator.Validate(PayrecToSend);
        //        return _service.ValidateUsername(username, out validationErrors);
        //    })
        //    .ConfigureAwait(false);

        //    if (!isValid)
        //    {
        //        /* Update the collection in the dictionary returned by the GetErrors method */
        //        _validationErrors[propertyKey] = validationErrors;
        //        /* Raise event to tell WPF to execute the GetErrors method */
        //        RaiseErrorsChanged(propertyKey);
        //    }
        //    else if (_validationErrors.ContainsKey(propertyKey))
        //    {
        //        /* Remove all errors for this property */
        //        _validationErrors.Remove(propertyKey);
        //        /* Raise event to tell WPF to execute the GetErrors method */
        //        RaiseErrorsChanged(propertyKey);
        //    }
        //}

        private readonly Dictionary<string, IList<ValidationFailure>> _validationErrors = new Dictionary<string, IList<ValidationFailure>>();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = (s, e) => {};
        private void RaiseErrorsChanged(string propertyName)
        {
            Ex.Log($"INotifyDataErrorInfo: RaiseErrorsChanged({propertyName})");
            ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }
        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            Ex.Log($"INotifyDataErrorInfo: IEnumerable GetErrors({propertyName})");
            if (string.IsNullOrEmpty(propertyName) || !_validationErrors.ContainsKey(propertyName))
            {                
                return null; 
            }
            Ex.Log($"INotifyDataErrorInfo: IEnumerable GetErrors({propertyName})={_validationErrors[propertyName][0]}");
            return _validationErrors[propertyName];
        }
        public bool HasErrors
        {
            get { Ex.Log($"INotifyDataErrorInfo: bool HasErrors count={_validationErrors.Count}"); return _validationErrors.Count > 0; }
        }
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
