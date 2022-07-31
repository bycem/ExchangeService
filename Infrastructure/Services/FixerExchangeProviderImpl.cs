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
    public class FixerExchangeProviderImpl : IExchangeProvider
    {
        private HttpClient _httpClient;

        public FixerExchangeProviderImpl(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetExchangeRateOutput> GetAsync(GetExchangeRateInput input)
        {
            var contentString = "";
            try
            {
                var currencyCodes = string.Join(",", input.TargetCurrencyCodes);
                var uri = $"/fixer/latest?symbols={currencyCodes}&base={input.SourceCurrency}";
                var httpResponse = await _httpClient.GetAsync(uri);

                contentString = await httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new ExternalServiceException( $"Fixer Error: {contentString}", httpResponse.StatusCode);
                }

                var result = await httpResponse.Content.ReadFromJsonAsync<ApiLayerGetLiveResponse>();
                var exchangeRates = Enumerable.Empty<CurrencyExchangeRate>();

                if (result != null)
                {
                    exchangeRates = result.Rates.Select(x => new CurrencyExchangeRate(x.Key, x.Value));
                }

                return new GetExchangeRateOutput("Fixer", exchangeRates);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"CurrencyLayer: Unexpected external service exception. HttpContent: {contentString}", ex);
            }
        }

        private sealed class ApiLayerGetLiveResponse
        {
            public string Base { get; set; }
            public Dictionary<string, double> Rates { get; set; }
            public bool Success { get; set; }
            public long Timestamp { get; set; }
        }
    }
}