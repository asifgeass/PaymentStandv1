using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Ioc;
using Logic;
using Logic.XML;

namespace WPFApp.ViewModels
{
    public class DynamicMenuWindowViewModel : BindableBase
    {
        public DynamicMenuWindowViewModel()
        {
            SetCurrMenuHeader = new DelegateCommand<string>(x => LabelCurrent = x);
            SetParentGroupHeader = new DelegateCommand<string>(x => LabelParentGroup = x);
            NextPage = new DelegateCommand<object>(NextPageCommand);
            LoadedCommand = new DelegateCommand(()=> NextPageCommand(null));
        }
        #region fields
        private object _selectedXmlArg;
        private string _labelGroup;
        private string _labelCurrent;
        private bool _isLoadingMenu;
        private PS_ERIP _responce;
        #endregion

        #region Properties
        public PS_ERIP Responce 
        {
            get => _responce;
            set => SetProperty(ref _responce, value); 
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
        public DelegateCommand<string> SetParentGroupHeader { get; }
        public DelegateCommand<string> SetCurrMenuHeader { get; }
        public DelegateCommand<object> NextPage { get; }
        public DelegateCommand LoadedCommand { get; }
        #endregion
        #region Command Methods
        private async void NextPageCommand(object param)
        {
            IsLoadingMenu = true;
            Responce = await ResponceBuilder.NextPage(param);
            IsLoadingMenu = false;
        }
        #endregion

        #region Methods Private
        #endregion
    }
}
