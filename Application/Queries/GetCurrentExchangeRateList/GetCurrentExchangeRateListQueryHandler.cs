using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Dtos.Services.ExchangeService;
using Domain.Interfaces.Services;
using MediatR;

namespace Application
{
    public class GetCurrentExchangeRateListQueryHandler : IRequestHandler<GetCurrencyExchangeRateListQuery,
        GetCurrentExchangeRateListResponse>
    {
        private readonly IExchangeProviderOrchestrator _exchangeProvider;

        public GetCurrentExchangeRateListQueryHandler(IExchangeProviderOrchestrator exchangeProvider)
        {
            _exchangeProvider = exchangeProvider;
        }

        public async Task<GetCurrentExchangeRateListResponse> Handle(GetCurrencyExchangeRateListQuery request,
            CancellationToken cancellationToken)
        {
            var result =
                await _exchangeProvider.GetAsync(new GetExchangeRateInput(request.SourceCurrency,
                    request.TargetCurrencies));

            return new GetCurrentExchangeRateListResponse(result.Provider,
                result.ExchangeRates.ToDictionary(x => x.Code, y => y.Amount));
        }
    }
}