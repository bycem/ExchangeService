using System;
using System.Collections.Generic;
using System.Linq;
using Application.Queries.GetExchangeRateListByFilter;
using Domain.Entities;
using FluentAssertions;

namespace UnitTests.Domain
{
    public class ExchangeTransactionTests
    {
        private IEnumerable<TargetCurrencyCalculation> _targetCurrencyCalculations;
        private Guid _id = Guid.NewGuid();
        private DateTime _date = DateTime.Now;

        public ExchangeTransactionTests()
        {
            _targetCurrencyCalculations = new List<TargetCurrencyCalculation>()
            {
                new TargetCurrencyCalculation("EUR", 10.0D)
            };
        }
        

        [TestCase("TRY", 10.0D, "Test")]
        [TestCase("EUR", 1500, "TestDENEME_12334345")]
        public void Exhchange_Transaction_SuccessTest(string sourceCurrency,
            double sourceCurrencyAmount,
            string provider)
        {
            var targetCurrencies = new List<TargetCurrencyCalculation>();
            targetCurrencies.Add(new TargetCurrencyCalculation("EUR", 10.0D));

            var transaction = new ExchangeTransaction(
                sourceCurrency, 
                sourceCurrencyAmount, 
                provider,
                targetCurrencies,
                _id,
                _date);

            transaction.Id.Should().Be(_id);
            transaction.CreateDate.Should().Be(_date);
            transaction.SourceCurrency.Should().Be(sourceCurrency);
            transaction.SourceCurrencyAmount.Should().Be(sourceCurrencyAmount);
            transaction.Provider.Should().Be(provider);
            transaction.TargetCurrencies.Should().BeEquivalentTo(targetCurrencies);
        }

        [TestCase("TY", 10.0D, "Test")]
        [TestCase("", 1500, "TestDENEME_12334345")]
        [TestCase("   ", 1500, "TestDENEME_12334345")]
        public void Throw_Exception_When_SourceCurrency_Invalid(string sourceCurrency,
            double sourceCurrencyAmount,
            string provider)
        {
            var targetCurrencies = new List<TargetCurrencyCalculation>();
            targetCurrencies.Add(new TargetCurrencyCalculation("EUR", 10.0D));
            
            var id = Guid.NewGuid();
            var date = DateTime.Now;

            Assert.Throws(Is.TypeOf<ArgumentException>()
                .And.Message.EqualTo("Source currency cannot be null and must be three characters"), () =>
            {
                var transaction = new ExchangeTransaction(
                    sourceCurrency,
                    sourceCurrencyAmount,
                    provider,
                    targetCurrencies,
                    id,
                    date);
            });
        }
        
        [TestCase("TRY", 0, "TestDENEME_12334345")]
        [TestCase("EUR", -5, "TestDENEME_12334345")]
        public void Throw_Exception_When_SourceAmount_Invalid(string sourceCurrency,
            double sourceCurrencyAmount,
            string provider)
        {
            Assert.Throws(Is.TypeOf<ArgumentException>()
                .And.Message.EqualTo("Source Currency amount must be bigger than zero"), () =>
            {
                var transaction = new ExchangeTransaction(
                    sourceCurrency,
                    sourceCurrencyAmount,
                    provider,
                    _targetCurrencyCalculations,
                    _id,
                    _date);
            });
        }
        
        
        
        
    }
}