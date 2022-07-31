using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Dtos.Services.ExchangeService;
using Domain.Interfaces.Services;

namespace Infrastructure.Services
{
    public class CurrencyDataExchangeProviderImpl : IExchangeProvider
    {
        private HttpClient _httpClient;

        public CurrencyDataExchangeProviderImpl(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetExchangeRateOutput> GetAsync(GetExchangeRateInput input)
        {
            var contentString = "";
            try
            {
                var currencyCodes = string.Join(",", input.TargetCurrencyCodes);
                var uri = $"/currency_data/live?source={input.SourceCurrency}&currencies={currencyCodes}";
                var httpResponse = await _httpClient.GetAsync(uri);

                contentString = await httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new ExternalServiceException($"Fixer Error: {contentString}", httpResponse.StatusCode);
                }

                var result = await httpResponse.Content.ReadFromJsonAsync<CurrencyLayerGetLiveResponse>();
                var exchangeRates = Enumerable.Empty<CurrencyExchangeRate>();

                if (result != null)
                {
                    exchangeRates = result.Quotes.Select(x => new CurrencyExchangeRate(x.Key.Substring(3, 3), x.Value));
                }

                return new GetExchangeRateOutput("CurrencyData", exchangeRates);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"CurrencyLayer: Unexpected external service exception. HttpContent: {contentString}", ex);
            }
        }

        private sealed class CurrencyLayerGetLiveResponse
        {
            public string Source { get; set; }
            public Dictionary<string, double> Quotes { get; set; }
            public bool Success { get; set; }
            public long Timestamp { get; set; }
        }
    }
}