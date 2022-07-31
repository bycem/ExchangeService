using System;
using System.Collections.Generic;

namespace Application.Queries.GetExchangeRateListByFilter
{
    public class GetExchangeRateListByFilterResponse
    {
        public GetExchangeRateListByFilterResponse(IEnumerable<TransactionDto> transactions)
        {
            Transactions = transactions;
        }

        public IEnumerable<TransactionDto> Transactions { get; }
    }

    public class TransactionDto
    {
        public TransactionDto(Guid id, string sourceCurrency, double sourceCurrencyAmount,string provider, DateTime transactionDate,
            IEnumerable<TargetCurrencyDto> targetCurrencies)
        {
            TransactionId = id;
            SourceCurrency = sourceCurrency;
            TransactionDate = transactionDate;
            TargetCurrencies = targetCurrencies;
            SourceCurrencyAmount = sourceCurrencyAmount;
            Provider = provider;
        }

        public Guid TransactionId { get; }

        public string SourceCurrency { get; }

        public double SourceCurrencyAmount { get; }
        public string Provider { get;  }
        public DateTime TransactionDate { get; }
        public IEnumerable<TargetCurrencyDto> TargetCurrencies { get; }
    }

    public class TargetCurrencyDto
    {
        public TargetCurrencyDto(string code, double calculatedAmount)
        {
            Code = code;
            CalculatedAmount = calculatedAmount;
        }

        public string Code { get; }
        public double CalculatedAmount { get; }
    }
}