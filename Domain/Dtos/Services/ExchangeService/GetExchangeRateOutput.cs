using System.Collections.Generic;

namespace Domain.Dtos.Services.ExchangeService
{
    public class GetExchangeRateOutput
    {
        public GetExchangeRateOutput(string provider, IEnumerable<CurrencyExchangeRate> exchangeRates)
        {
            Provider = provider;
            ExchangeRates = exchangeRates;
        }

        public string Provider { get;}
        public IEnumerable<CurrencyExchangeRate> ExchangeRates { get; }
    }
}