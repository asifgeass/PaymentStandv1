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
        public DynamicMenuWindowViewModel()
        {
            SetCurrMenuHeaderCommand = new DelegateCommand<string>(x => LabelCurrent = x);
            SetParentGroupHeaderCommand = new DelegateCommand<string>(x => LabelParentGroup = x);
            NextPageCommand = new DelegateCommand<object>(NextPage);
            LoadedCommand = new DelegateCommand(()=> NextPage(null));
        }
        #region fields
        private object _selectedXmlArg;
        private string _labelGroup;
        private string _labelCurrent;
        private bool _isLoadingMenu;
        private PS_ERIP _responce;
        private PayRecord _payrecToSend;
        private PayRecord _test;
        #endregion

        #region Properties
        public PS_ERIP Responce 
        {
            get => _responce;
            set => SetProperty(ref _responce, value, ResponseChangedEvent); 
        }
        public PayRecord PayrecToSend
        {
            get => _payrecToSend;
            set => SetProperty(ref _payrecToSend, value);
        }
        public PayRecord Test
        {
            get => _test;
            set => SetProperty(ref _test, value);
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
        public DelegateCommand<object> NextPageCommand { get; }
        public DelegateCommand LoadedCommand { get; }
        #endregion
        #region Command Methods
        private async void NextPage(object param)
        {
            IsLoadingMenu = !IsLoadingMenu;
            //IsLoadingMenu = true;
            Responce = await ResponceBuilder.NextPage(param);
            //IsLoadingMenu = false;
        }
        #endregion

        #region Methods Private
        #endregion
    }
}
