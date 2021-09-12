namespace BTC.API.Models
{
    public class CurrencyInfo
    {
        public string Time { get; set; }
        public string AssetIdBase { get; set; }
        public string AssetIdQuote { get; set; }
        public decimal Rate { get; set; }
    }
}
