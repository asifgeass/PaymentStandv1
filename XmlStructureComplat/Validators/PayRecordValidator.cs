using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlStructureComplat.Validators
{
    public class PayRecordValidator : AbstractValidator<PayRecord>
    {
        public PayRecordValidator()
        {
            RuleFor(payrec => payrec.Summa)
                .NotNull()
                .NotEmpty()
                .NotEqual("0.00").WithMessage("Сумма должна быть > 0")
                .NotEqual("0,00")
                .NotEqual("0")
                .NotEqual("0.0")
                .NotEqual("0,0")
                .Matches(@"^.{0}\d+[\.,]?\d{0,2}.{0}$").WithMessage("Должно быть число, с дробной частью не более 2 цифры")
                ;



        }
    }
}
