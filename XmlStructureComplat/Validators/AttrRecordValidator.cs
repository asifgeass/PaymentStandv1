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
        private int temp;
        public AttrRecordValidator()
        {
            When(attr => attr.Mandatory != null && attr.Mandatory == "1", () =>
            {
                RuleFor(attr => attr.Value)
                    .NotNull().WithName("Значение")
                    .NotEmpty().WithName("Значение")
                    ;
            });

            When(x => x.MinLength != null && int.TryParse(x.MinLength, out temp) && !string.IsNullOrEmpty(x.Value), () =>
            {
                RuleFor(attr => attr.Value.Length)
                    .GreaterThanOrEqualTo(attr => int.Parse(attr.MinLength)).WithName("Длина");
            });

            When(x => x.MaxLength != null && int.TryParse(x.MaxLength, out temp) && !string.IsNullOrEmpty(x.Value), () =>
            {
                RuleFor(attr => attr.Value.Length)
                    .LessThanOrEqualTo(attr => int.Parse(attr.MaxLength)).WithName("Длина");
            });

            //RuleFor(attr => attr.Value)
            //    .MinimumLength(x=>x.).WithName("Значение2")
            //    .When(attr => !string.IsNullOrEmpty(attr.MinLength) 
            //    && int.Parse(attr.MinLength)
            //    && attr.Value != null);

            //RuleFor(attr => attr.Value)
            //    .MaximumLength(int.Parse(attr.MaxLength) ).WithName("Значени3")
            //    .When(attr => !string.IsNullOrEmpty(attr.MaxLength) 
            //    && int.Parse(attr.MaxLength)
            //    && attr.Value != null);

        }
    }
}
