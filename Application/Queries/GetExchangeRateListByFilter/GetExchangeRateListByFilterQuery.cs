using System;
using MediatR;

namespace Application.Queries.GetExchangeRateListByFilter
{
    public class GetExchangeRateListByFilterQuery : IRequest<GetExchangeRateListByFilterResponse>
    {
        public GetExchangeRateListByFilterQuery(DateTime? startDate, DateTime? endDate, Guid? transactionId)
        {
            StartDate = startDate;
            EndDate = endDate;
            TransactionId = transactionId;
        }

        public Guid? TransactionId { get; }
        public DateTime? StartDate { get;  }
        public DateTime? EndDate { get; }
    }
}