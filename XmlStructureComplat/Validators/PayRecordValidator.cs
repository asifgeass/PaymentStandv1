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
                .NotEqual("0.00")
                .NotEqual("0,00")
                .NotEqual("0")
                .NotEqual("0.0")
                .NotEqual("0,0")
                .WithMessage("Сумма должна быть > 0");

            RuleFor(payrec => payrec.Summa)
                .Matches(@"\d+[\.,]?\d{0,2}") //numeric Summa 
                .WithMessage("Должно быть число, с дробной частью не более 2 цифры"); 

        }
    }
}
