using System.Collections.Generic;

namespace Application
{
    public class GetCurrentExchangeRateListResponse
    {
        public GetCurrentExchangeRateListResponse(string provider, Dictionary<string, double> exchangeRateList)
        {
            Provider = provider;
            ExchangeRateList = exchangeRateList;
        }

        public string Provider { get;  }

        public Dictionary<string,double> ExchangeRateList { get;  }
    }
}