using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Dtos.Services.ExchangeService;
using Domain.Interfaces.Services;

namespace Infrastructure.Services
{
    public class ExchangeProviderOrchestratorImpl : IExchangeProviderOrchestrator
    {
        private readonly IEnumerable<IExchangeProvider> _providers;
        private const int Timeout = 1000;

        public ExchangeProviderOrchestratorImpl(IEnumerable<IExchangeProvider> providers)
        {
            _providers = providers;
        }

        public async Task<GetExchangeRateOutput> GetAsync(GetExchangeRateInput input)
        {
            foreach (var provider in _providers)
            {
                var task = provider.GetAsync(input);
                if (await Task.WhenAny(task, Task.Delay(Timeout, CancellationToken.None)) == task)
                {
                    return await task;
                }
            }

            throw new ExternalServiceException("Providers Failed", HttpStatusCode.RequestTimeout);
        }
    }
}