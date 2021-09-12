using CoinRateService.Helpers;
using CoinRateService.Interfaces;
using CoinRateService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using static CoinRateService.Helpers.Dictionaries;

namespace CoinRateService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly ICoinApiRequestSender _coinApiRequestSender;
        private readonly Func<string, string, string> _subPath = (idBase, idQuote)
            => $"v1/exchangerate/{idBase}/{idQuote}";

        public RateController(ICoinApiRequestSender coinApiRequestSender)
        {
            _coinApiRequestSender = coinApiRequestSender;
        }

        [HttpGet("btc/uah")]
        public async Task<ActionResult<CurrencyInfo>> GetBtcRateInUah()
        {
            var subPath = BuildSubPath(Currency.BTC, Currency.UAH);
            var result = await _coinApiRequestSender.SendGetRequest(subPath);

            if (result == null)
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, "Couldn't get response from coinapi.io");

            return Ok(result);
        }

        private string BuildSubPath(Currency crypro, Currency fiat)
        {
            return _subPath.Invoke(Currencies[crypro], Currencies[fiat]);
        }
    }
}
