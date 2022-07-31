using System.Collections.Generic;
using FluentValidation;

namespace Application.Common
{
    public static class CommonValidators {
        
        public static IRuleBuilderOptions<T, string> CurrencyCodeValidation<T>(this IRuleBuilder<T, string> ruleBuilder) {
            return ruleBuilder.NotEmpty().Length(3)
                .WithMessage("{PropertyValue} is invalid. {PropertyName} cannot be null and length must be 3");
        }
    }

}