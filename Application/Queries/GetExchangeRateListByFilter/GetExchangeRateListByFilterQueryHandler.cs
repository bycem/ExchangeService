using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Queries.GetExchangeRateListByFilter
{
    public class GetExchangeRateListByFilterQueryHandler : IRequestHandler<GetExchangeRateListByFilterQuery,
        GetExchangeRateListByFilterResponse>
    {
        private IExchangeTransactionRepository _transactionRepository;

        public GetExchangeRateListByFilterQueryHandler(IExchangeTransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<GetExchangeRateListByFilterResponse> Handle(GetExchangeRateListByFilterQuery request,
            CancellationToken cancellationToken)
        {
            var transactions = new List<TransactionDto>();
            if (request.TransactionId.HasValue)
            {
                var response = await _transactionRepository.GetByIdAsync(request.TransactionId.Value);
                if (response == null)
                    throw new NotFoundException(nameof(ExchangeTransaction));

                var transaction = new TransactionDto(response.Id, response.SourceCurrency,
                    response.SourceCurrencyAmount, response.Provider, response.CreateDate,
                    response.TargetCurrencies.Select(x => new TargetCurrencyDto(x.Code, x.CalculatedAmount)));

                transactions.Add(transaction);
            }
            else
            {
                var response =
                    await _transactionRepository.GetByDateRangeAsync(request.StartDate.Value,
                        request.EndDate.Value.AddDays(1).AddSeconds(-1));

                transactions = response.Select(x =>
                    new TransactionDto(x.Id, x.SourceCurrency, x.SourceCurrencyAmount, x.Provider, x.CreateDate,
                        x.TargetCurrencies.Select(x =>
                            new TargetCurrencyDto(x.Code, x.CalculatedAmount)))).ToList();
            }

            var result = new GetExchangeRateListByFilterResponse(transactions);
            return result;
        }
    }
}