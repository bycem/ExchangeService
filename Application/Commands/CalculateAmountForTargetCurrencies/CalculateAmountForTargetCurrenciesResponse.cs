using System;
using System.Collections.Generic;

namespace Application.Commands.CalculateAmountForTargetCurrencies
{
    public class CalculateAmountForTargetCurrenciesResponse
    {
        public CalculateAmountForTargetCurrenciesResponse(Guid transactionId, Dictionary<string, double> exchangeRateList)
        {
            TransactionId = transactionId;
            ExchangeRateList = exchangeRateList;
        }

        public Guid TransactionId { get;  }
        public Dictionary<string,double> ExchangeRateList { get; }
    }
}