using System.Linq;
using Application.Common;
using FluentValidation;

namespace Application.Commands.CalculateAmountForTargetCurrencies
{
    public class CalculateAmountForTargetCurrenciesCommandValidator : AbstractValidator<CalculateAmountForTargetCurrenciesCommand>
    {
        public CalculateAmountForTargetCurrenciesCommandValidator()
        {
            RuleFor(x => x.SourceCurrency)
                .CurrencyCodeValidation();

            RuleFor(x => x.TargetCurrencies)
                .ForEach(x => x.CurrencyCodeValidation());

            RuleFor(x => x.SourceCurrencyAmount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than zero");
            
            RuleFor(x=>x.TargetCurrencies)
                .Must(x => x.Distinct().Count() == x.Count())
                .WithMessage("Recurring target currencies not allowed");
        }
    }
}