using System.Collections.Generic;
using Domain.Interfaces.Services;

namespace Domain.Dtos.Services.ExchangeService
{
    public class GetExchangeRateInput
    {
        public GetExchangeRateInput(string sourceCurrency, IEnumerable<string> targetCurrencyCodes)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrencyCodes = targetCurrencyCodes;
        }

        public string SourceCurrency { get;  }
        public IEnumerable<string> TargetCurrencyCodes { get;  }
    }
}