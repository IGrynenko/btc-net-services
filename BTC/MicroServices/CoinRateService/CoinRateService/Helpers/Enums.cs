using System.Collections.Generic;

namespace CoinRateService.Helpers
{
    public enum Currency
    {
        BTC,
        USD,
        UAH
    }

    public static class Dictionaries
    {
        public static Dictionary<Currency, string> Currencies = new Dictionary<Currency, string>
        {
            { Currency.BTC, "BTC" },
            { Currency.USD, "USD" },
            { Currency.UAH, "UAH" }
        };
    }
}
