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
                .NotNull()
                .NotEmpty()
                .When(attr => attr.Mandatory != null && attr.Mandatory == "1");

            RuleFor(attr => attr.Value)
                .NotNull()
                .NotEmpty()
                .MinimumLength(minLength)
                .When(attr => !string.IsNullOrEmpty(attr.MinLength) 
                && int.TryParse(attr.MinLength, out minLength));

            RuleFor(attr => attr.Value)
                .NotNull()
                .NotEmpty()
                .MinimumLength(maxLength)
                .When(attr => !string.IsNullOrEmpty(attr.MaxLength) 
                && int.TryParse(attr.MaxLength, out maxLength));

        }
    }
}
