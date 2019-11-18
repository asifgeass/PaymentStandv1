using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlStructureComplat.Validators
{
    public class AttrRecordValidator : AbstractValidator<AttrRecord>
    {
        private int minLength;
        private int maxLength;
        public AttrRecordValidator()
        {
            RuleFor(attr => attr.Value)
                .NotNull().WithName("Значение")
                .NotEmpty().WithName("Значение")
                .When(attr => attr.Mandatory != null && attr.Mandatory == "1");

            RuleFor(attr => attr.Value)
                .MinimumLength(minLength).WithName("Значение")
                .When(attr => !string.IsNullOrEmpty(attr.MinLength) 
                && int.TryParse(attr.MinLength, out minLength)
                && attr.Value != null);

            RuleFor(attr => attr.Value)
                .MaximumLength(maxLength).WithName("Значение")
                .When(attr => !string.IsNullOrEmpty(attr.MaxLength) 
                && int.TryParse(attr.MaxLength, out maxLength)
                && attr.Value != null);

        }
    }
}
