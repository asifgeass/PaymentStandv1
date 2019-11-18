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
                  .NotNull().WithName("Сумма")
                  .NotEmpty().WithName("Сумма")                  
                  .Matches(@"^\s*\d+[\.,]?\d{0,2}\s*$")
                  .WithMessage("Должно быть число, с дробной частью не более 2 цифры")
                  .Must(sum =>decimal.Parse(sum) != 0)
                  .WithMessage("Нельзя оплатить сумму 0")
                  ;
        }

    }
}
