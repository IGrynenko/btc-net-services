using CoinRateService.Models;
using System.Threading.Tasks;

namespace CoinRateService.Interfaces
{
    public interface ICoinApiRequestSender
    {
        Task<CurrencyInfo> SendGetRequest(string subPath);
    }
}