using System.Collections.Generic;

namespace Api.Dtos
{
    public class ExchangeListInput
    {
        public ExchangeListInput(IEnumerable<string> targetCurrencies, double amount)
        {
            TargetCurrencies = targetCurrencies;
            Amount = amount;
        }

        public IEnumerable<string> TargetCurrencies { get; set; }
        public double Amount { get; set; }
    }
}