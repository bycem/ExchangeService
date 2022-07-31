using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Dtos.Services.ExchangeService;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using MediatR;

namespace Application.Commands.CalculateAmountForTargetCurrencies
{
    public class CalculateAmountForTargetCurrenciesCommandHandler : IRequestHandler<
        CalculateAmountForTargetCurrenciesCommand,
        CalculateAmountForTargetCurrenciesResponse>
    {
        private readonly IExchangeProviderOrchestrator _exchangeProvider;
        private readonly IExchangeTransactionRepository _exchangeTransactionRepository;

        public CalculateAmountForTargetCurrenciesCommandHandler(
            IExchangeProviderOrchestrator exchangeProvider,
            IExchangeTransactionRepository exchangeTransactionRepository)
        {
            _exchangeProvider = exchangeProvider;
            _exchangeTransactionRepository = exchangeTransactionRepository;
        }

        public async Task<CalculateAmountForTargetCurrenciesResponse> Handle(
            CalculateAmountForTargetCurrenciesCommand request, CancellationToken cancellationToken)
        {
            var rates = await _exchangeProvider.GetAsync(new GetExchangeRateInput(request.SourceCurrency,
                request.TargetCurrencies));

            var resultList = new Dictionary<string, double>();
            foreach (var currencyExchangeRate in rates.ExchangeRates)
            {
                resultList[currencyExchangeRate.Code] = request.SourceCurrencyAmount * currencyExchangeRate.Amount;
            }

            var targetCurrencies = resultList.Select(x =>
                new TargetCurrencyCalculation(x.Key, x.Value));

            var transactionId = Guid.NewGuid();
            await _exchangeTransactionRepository.CreateAsync(new ExchangeTransaction(request.SourceCurrency,
                request.SourceCurrencyAmount,
                rates.Provider,
                targetCurrencies,
                transactionId,
                DateTime.Now));

            return new CalculateAmountForTargetCurrenciesResponse(transactionId, resultList);
        }
    }
}