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

            When(attr => attr.Type != null, () =>
            {
                RuleFor(attr => attr.Value)
                    .Matches(@"^\d+[\.]?\d*$").WithMessage("Не является числом.")
                    .When(attr => attr.Type == "R")
                    ;
                RuleFor(attr => attr.Value)
                    .Matches(@"^\d+[\.]?\d*$").WithMessage("Не является числом.")
                    .Matches(@"^\d+$").WithMessage("Не является целочисленным числом.")
                    .When(attr => attr.Type == "I")
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

            When(attr => attr.Format != null, () =>
            {
                RuleFor(attr => attr.Value)
                    .Matches(attr => attr.Format).WithMessage("Значение не соответствует формату.")
                    ;
            });

        }
    }
}
