using System.Collections.Generic;
using MediatR;

namespace Application
{
    public class GetCurrencyExchangeRateListQuery : IRequest<GetCurrentExchangeRateListResponse>
    {
        public GetCurrencyExchangeRateListQuery(string sourceCurrency, IEnumerable<string> targetCurrencies)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrencies = targetCurrencies;
        }

        public string SourceCurrency { get;  }
        public IEnumerable<string> TargetCurrencies { get;  }
    }
}