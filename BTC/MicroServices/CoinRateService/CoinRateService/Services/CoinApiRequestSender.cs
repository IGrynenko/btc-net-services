using CoinRateService.Helpers;
using CoinRateService.Interfaces;
using Microsoft.Extensions.Options;
using RestSharp;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using CoinRateService.Models;

namespace CoinRateService.Services
{
    public class BtcRequestSender : ICoinApiRequestSender, IDisposable
    {
        private readonly IOptions<CoinApiSettings> _coinApiSettings;
        private IRestClient _client;

        public BtcRequestSender(IOptions<CoinApiSettings> coinApiSettings, IRestClient client)
        {
            _coinApiSettings = coinApiSettings;
            _client = client;
            _client.BaseUrl = new Uri(coinApiSettings.Value.Path);
        }

        public async Task<CurrencyInfo> SendGetRequest(string subPath = null)
        {
            var request = new RestRequest(Method.GET);

            if (!string.IsNullOrEmpty(subPath))
                _client.BaseUrl = new Uri(_coinApiSettings.Value.Path + subPath);

            request.AddHeader("X-CoinAPI-Key", _coinApiSettings.Value.Key);
            var response = await _client.ExecuteAsync(request);

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var currencyInfo = JsonConvert.DeserializeObject<CurrencyInfo>(response.Content);
                return currencyInfo;
            }

            return null;
        }

        public void Dispose()
        {
            _client = null;
        }
    }
}
