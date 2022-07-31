using System;
using System.Collections.Generic;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public sealed  class TargetCurrencyCalculation:BaseValueObject
    {
        public TargetCurrencyCalculation(string code, double calculatedAmount)
        {
            if (string.IsNullOrEmpty(code) || code.Length != 3)
                throw new ArgumentException("Currency code cannot be null and must be three characters");
            
            if (calculatedAmount <= 0)
                throw new ArgumentException("CalculatedAmount must be greater than zero");

            Code = code;
            CalculatedAmount = calculatedAmount;
        }
        public string Code { get; }
        public double CalculatedAmount { get; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
            yield return CalculatedAmount; 
        }
    }
}