using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace ExternalServices.Repositories
{
    public class ExchangeTransactionRepository : IExchangeTransactionRepository
    {
        private readonly List<ExchangeTransaction> _transactions = new();

        public Task CreateAsync(ExchangeTransaction transaction)
        {
            _transactions.Add(transaction);

            return Task.CompletedTask;
        }

        public Task<IEnumerable<ExchangeTransaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return Task.FromResult(_transactions.Where(x => x.CreateDate >= startDate && x.CreateDate <= endDate));
        }

        public Task<ExchangeTransaction> GetByIdAsync(Guid transactionId)
        {
            return Task.FromResult(_transactions.FirstOrDefault(x => x.Id == transactionId));
        }
    }
}