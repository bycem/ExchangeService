using System.Collections.Generic;
using MediatR;

namespace Application.Commands.CalculateAmountForTargetCurrencies
{
    public class CalculateAmountForTargetCurrenciesCommand:IRequest<CalculateAmountForTargetCurrenciesResponse>
    {
        public CalculateAmountForTargetCurrenciesCommand(string sourceCurrency, double sourceCurrencyAmount, IEnumerable<string> targetCurrencies)
        {
            SourceCurrency = sourceCurrency;
            SourceCurrencyAmount = sourceCurrencyAmount;
            TargetCurrencies = targetCurrencies;
        }

        public string SourceCurrency { get;  }
        public double SourceCurrencyAmount { get;  }
        public IEnumerable<string> TargetCurrencies { get;  }
    }
}