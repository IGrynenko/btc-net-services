using System;

namespace BTC.API.Helpers
{
    public class TokenException : ArgumentException
    {
        public TokenException(string message)
            : base(message)
        { }
    }
}
