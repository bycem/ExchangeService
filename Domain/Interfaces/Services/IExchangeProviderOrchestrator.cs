using System.Threading.Tasks;
using Domain.Dtos.Services.ExchangeService;

namespace Domain.Interfaces.Services
{
    public interface IExchangeProviderOrchestrator
    {
        Task<GetExchangeRateOutput> GetAsync(GetExchangeRateInput input);
    }
}