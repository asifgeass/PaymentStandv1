using ExceptionManager;
using FluentValidation.Results;
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
    public class ValidatableBindableBase : BindableBase, INotifyDataErrorInfo
    {
        public void Validate()
        {

        }
        public bool ValidateResult(ValidationResult valResult, [CallerMemberName] string propName = null)
        {
            if (valResult == null) return true;

            if (!valResult.IsValid)
            {
                List<string> newList = new List<string>();
                foreach (ValidationFailure item in valResult.Errors)
                {
                    newList.Add(item.ErrorMessage);
                }
                _validationErrors[propName] = newList;
                RaiseErrorsChanged(propName);
            }
            else if (_validationErrors.ContainsKey(propName))
            {
                /* Remove all errors for this property */
                _validationErrors.Remove(propName);
                /* Raise event to tell WPF to execute the GetErrors method */
                RaiseErrorsChanged(propName);
            }
            Ex.Log($"ValidatableBindableBase.ValidateResult({valResult}, {propName})");
            return valResult.IsValid;
        }
        #region INotifyDataErrorInfo members 2013 Magnus Montin

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

        private readonly Dictionary<string, IList<string>> _validationErrors = new Dictionary<string, IList<string>>();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = (s, e) => { };
        private void RaiseErrorsChanged(string propertyName)
        {
            Ex.Log($"INotifyDataErrorInfo.RaiseErrorsChanged({propertyName})");
            ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }
        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            Ex.Log($"INotifyDataErrorInfo: GetErrors({propertyName})");
            if (string.IsNullOrEmpty(propertyName) || !_validationErrors.ContainsKey(propertyName))
            {
                return null;
            }
            Ex.Log($"INotifyDataErrorInfo: GetErrors({propertyName})={_validationErrors[propertyName][0]}");
            return _validationErrors[propertyName];
        }
        public bool HasErrors
        {
            get { /*Ex.Log($"INotifyDataErrorInfo: bool HasErrors count={_validationErrors.Count}");*/ return _validationErrors.Count > 0; }
        }
        #endregion
    }
}
