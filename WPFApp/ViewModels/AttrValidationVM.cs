using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlStructureComplat;
using XmlStructureComplat.Validators;

namespace WPFApp.ViewModels
{
    public class AttrValidationVM : ValidatableBindableBase
    {
        private readonly AttrRecordValidator attrValidator = new AttrRecordValidator();
        private AttrRecord _attrRecord = new AttrRecord();

        #region ctor
        public AttrValidationVM() { }
        public AttrValidationVM(AttrRecord AttrRecord) : this()
        {
            _attrRecord = AttrRecord;
        }
        #endregion

        public AttrRecord AttrRecord
        {
            get => _attrRecord;
            set => SetProperty(ref _attrRecord, value);
        }

        public string ValueAttrRecord
        {
            get => AttrRecord.Value;
            set
            {
                AttrRecord.Value = value;                
                var valResult = attrValidator.Validate(AttrRecord);
                var isValid = ValidateResult(valResult);
                RaisePropertyChanged();
            }
        }
    }
}
