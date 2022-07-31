using System;
using FluentValidation;

namespace Application.Queries.GetExchangeRateListByFilter
{
    public class GetExchangeRateListByFilterQueryValidator : AbstractValidator<GetExchangeRateListByFilterQuery>
    {
        public GetExchangeRateListByFilterQueryValidator()
        {
            RuleFor(x => x.TransactionId)
                .NotEqual(Guid.Empty)
                .NotEmpty()
                .When(x => !x.StartDate.HasValue && !x.EndDate.HasValue)
                .WithMessage("Invalid transactionId");

            RuleFor(x => x.TransactionId)
                .Empty()
                .When(x => x.StartDate.HasValue || x.EndDate.HasValue)
                .WithMessage("Only one of the inputs shall be provided(TransactionId or DateRange).");
            
            RuleFor(x => x.StartDate)
                .NotEmpty()
                .When(x => !x.TransactionId.HasValue || x.EndDate.HasValue)
                .WithMessage("Startdate cannot be null");
            
            RuleFor(x => x.EndDate)
                .NotEmpty()
                .When(x => !x.TransactionId.HasValue || x.StartDate.HasValue)
                .WithMessage("EndDate cannot be null");

            RuleFor(x => x)
                .Must(x => x.StartDate.Value.Date <= x.EndDate.Value.Date)
                .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
                .WithMessage("Enddate must be greater than StartDate");
        }
    }
}