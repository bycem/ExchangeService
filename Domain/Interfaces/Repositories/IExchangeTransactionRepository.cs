using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IExchangeTransactionRepository
    {
        Task CreateAsync(ExchangeTransaction transaction);
        Task<IEnumerable<ExchangeTransaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<ExchangeTransaction?> GetByIdAsync(Guid transactionId);
    }
    
}