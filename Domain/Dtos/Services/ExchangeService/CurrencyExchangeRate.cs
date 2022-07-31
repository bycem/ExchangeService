namespace Domain.Dtos.Services.ExchangeService
{
    public class CurrencyExchangeRate
    {
        public CurrencyExchangeRate(string code, double amount)
        {
            Code = code;
            Amount = amount;
        }

        public string Code { get;  }
        public double Amount { get;  }
    }
}