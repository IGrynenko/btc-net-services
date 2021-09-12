using BTC.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BTC.API.Controllers
{
    [Route("api/btcRate")]
    [ApiController]
    public class BtcRateController : ControllerBase
    {
        private readonly IRestClient _client;
        private readonly IConfiguration _configuration;

        public BtcRateController(RestClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<CurrencyInfo>> GetBtcRateInUah()
        {
            _client.BaseUrl = new Uri(_configuration.GetSection("Services:CoinRateService:Url").Value);
            var request = new RestRequest(_configuration.GetSection("Services:CoinRateService:BtcToUah").Value, Method.GET);
            var result = await _client.GetAsync<CurrencyInfo>(request);

            if (result != null)
            {
                if (result is IRestResponse response)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                        return Ok(result);

                    else if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                        return StatusCode((int)HttpStatusCode.ServiceUnavailable, "Coin Service is temporarily unavailable");
                }
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
