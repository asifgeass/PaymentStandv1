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
    public class LookupVM : BindableBase
    {
        #region fields
        private Lookup _lookup;
        #endregion

        #region ctor
        public LookupVM()
        {
            SelectLookupCommand = new DelegateCommand<LookupItem>(SetLookup);
        }
        #endregion
        public Lookup Lookup
        {
            get => _lookup;
            set => SetProperty(ref _lookup, value);
        }

        public DelegateCommand<LookupItem> SelectLookupCommand { get; }

        private void SetLookup(LookupItem arg)
        {
            if (Lookup == null) { Lookup = new Lookup(); }
            Lookup.SelectedItem = arg;
        }
    }
}
