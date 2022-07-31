using System.Linq;
using Application.Common;
using FluentValidation;

namespace Application
{
    public class GetCurrentExchangeRateListQueryValidator : AbstractValidator<GetCurrencyExchangeRateListQuery>
    {
        public GetCurrentExchangeRateListQueryValidator()
        {
            RuleFor(x => x.SourceCurrency).
                CurrencyCodeValidation();
            
            RuleFor(x => x.TargetCurrencies)
                .ForEach(targetCurrency => targetCurrency.CurrencyCodeValidation());

            RuleFor(x => x.TargetCurrencies)
                .Must(x => x.Distinct().Count() == x.Count())
                .WithMessage("Recurring target currencies not allowed");
        }
    }
}