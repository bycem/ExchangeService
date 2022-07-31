using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public sealed class ExchangeTransaction : BaseEntity
    {
        public string SourceCurrency { get; }
        public double SourceCurrencyAmount { get; }
        public string Provider { get; }

        public IEnumerable<TargetCurrencyCalculation> TargetCurrencies { get; }

        public ExchangeTransaction(string sourceCurrency,
            double sourceCurrencyAmount,
            string provider,
            IEnumerable<TargetCurrencyCalculation> targetCurrencies,
            Guid id,
            DateTime createDate) : base(id, createDate)
        {
            if (string.IsNullOrWhiteSpace(sourceCurrency) || sourceCurrency.Length != 3)
                throw new ArgumentException("Source currency cannot be null and must be three characters");

            if (targetCurrencies == null || !targetCurrencies.Any())
                throw new ArgumentException("Target currencies cannot be null or empty");

            if (sourceCurrencyAmount <= 0)
                throw new ArgumentException("Source Currency amount must be bigger than zero");

            if (string.IsNullOrWhiteSpace(provider))
                throw new ArgumentException("Provider cannot be null or empty");
            
            SourceCurrency = sourceCurrency;
            TargetCurrencies = targetCurrencies;
            SourceCurrencyAmount = sourceCurrencyAmount;
            Provider = provider;
        }
    }
}