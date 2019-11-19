using FluentValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlStructureComplat.Validators
{
    public class PayRecordValidator : AbstractValidator<PayRecord>
    {
        decimal temp;
        public PayRecordValidator()
        {
            RuleFor(payrec => payrec.Summa)               
                  .NotNull().WithName("Сумма")
                  .NotEmpty().WithName("Сумма")                  
                  .Matches(@"^\s*\d+[\.,]?\d{0,2}\s*$")
                  .WithMessage("Должно быть число, с дробной частью не более 2 цифры")
                  ;
            When(payrec=> decimal.TryParse(payrec.Summa.Replace(',', '.'), NumberStyles.Currency, CultureInfo.InvariantCulture, out temp), () =>
            {
                RuleFor(payrec => payrec.Summa)
                      .Must(sum => decimal.Parse(sum.Replace(',', '.'), NumberStyles.Currency, CultureInfo.InvariantCulture) != 0)
                      .WithMessage("Нельзя оплатить сумму 0")
                      ;
            });
        }

    }
}
