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
        private AttrRecord _attrRecord;
        public AttrRecord AttrRecord
        {
            get => AttrRecord;
            set => SetProperty(ref _attrRecord, value);
        }

        public string ValueAttrRecord
        {
            get => AttrRecord.Value;
            set
            {
                AttrRecord.Value = value;
                RaisePropertyChanged();
                var valResult = attrValidator.Validate(AttrRecord);
            }
        }
    }
}
