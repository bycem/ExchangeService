using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Commands.CalculateAmountForTargetCurrencies;
using Domain.Dtos.Services.ExchangeService;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using FluentAssertions;
using Moq;

namespace UnitTests.Commands.CalculateAmountForTargetCurrencies
{
    [TestFixture]
    public class SuccessTest
    {
        private readonly Mock<IExchangeProviderOrchestrator> _mockExchangeProvider = new();
        private readonly Mock<IExchangeTransactionRepository> _mockExchangeTransactionRepository = new();

        private CalculateAmountForTargetCurrenciesResponse _response;

        private string _sourceCurrency = "TRY";
        private double _sourceCurrencyAmount = 25;

        private string _targetCurrency1 = "USD";
        private string _targetCurrency2 = "TRY";

        private double _target1Amount = 17.91D;
        private double _target2Amount = 1;

        private DateTime _operationDate;

        [OneTimeSetUp]
        public async Task Setup()
        {
            SetupMocks();
            var _handler = new CalculateAmountForTargetCurrenciesCommandHandler(_mockExchangeProvider.Object,
                _mockExchangeTransactionRepository.Object);


            _response = await _handler.Handle(
                new CalculateAmountForTargetCurrenciesCommand(_sourceCurrency, _sourceCurrencyAmount,
                    new[] {_targetCurrency1, _targetCurrency2})
                , CancellationToken.None);
            _operationDate = DateTime.Now;
        }


        [Test]
        public void Exchange_Provider_Should_Be_Verified()
        {
            _mockExchangeProvider.Verify(x =>
                x.GetAsync(
                    It.Is<GetExchangeRateInput>(input =>
                        input.SourceCurrency == _sourceCurrency &&
                        input.TargetCurrencyCodes.SequenceEqual(new List<string>() {_targetCurrency1, _targetCurrency2})
                    )), Times.Once);
        }

        [Test]
        public void ExchangeTransaction_Repository_Should_Be_Verified()
        {
            _mockExchangeTransactionRepository.Verify(x =>
                x.CreateAsync(
                    It.Is<ExchangeTransaction>(input =>
                        input.SourceCurrency == _sourceCurrency &&
                        input.SourceCurrencyAmount == _sourceCurrencyAmount &&
                        input.Id != Guid.Empty &&
                        input.TargetCurrencies.Any(x => x.Code == _targetCurrency1
                                                        && x.CalculatedAmount == _target1Amount * _sourceCurrencyAmount) &&
                        input.TargetCurrencies.Any(x => x.Code == _targetCurrency2
                                                        && x.CalculatedAmount ==
                                                        _target2Amount * _sourceCurrencyAmount)
                    )),Times.Once);
        }

        [Test]
        public void response_should_be_correct()
        {
            _response.TransactionId.Should().NotBeEmpty();
            _response.ExchangeRateList.Should().HaveCount(2);
        }

        [Test]
        public void TargetCurrency_1_should_be_correct()
        {
            _response.ExchangeRateList.ContainsKey(_targetCurrency1).Should().BeTrue();
            _response.ExchangeRateList[_targetCurrency1].Should().Be(_target1Amount * _sourceCurrencyAmount);
        }
        
        [Test]
        public void TargetCurrency_2_should_be_correct()
        {
            _response.ExchangeRateList.ContainsKey(_targetCurrency2).Should().BeTrue();
            _response.ExchangeRateList[_targetCurrency2].Should().Be(_target2Amount * _sourceCurrencyAmount);
        }

        private void SetupMocks()
        {
            _mockExchangeProvider.Setup(x => x.GetAsync(
                It.IsAny<GetExchangeRateInput>()
            )).Returns(Task.FromResult(new GetExchangeRateOutput("Any",new[]
            {
                new CurrencyExchangeRate(_targetCurrency1, _target1Amount),
                new CurrencyExchangeRate(_targetCurrency2, _target2Amount)
            })));
        }
    }
}