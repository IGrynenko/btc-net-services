using BTC.API.Helpers;
using BTC.API.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BTC.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtTokenSettings _jwtTokenSettings;
        private readonly double defaultTime = 100d;

        public TokenService(IOptions<JwtTokenSettings> options)
        {
            _jwtTokenSettings = options.Value;
        }

        public string GenerateJwtToken()
        {
            if (string.IsNullOrEmpty(_jwtTokenSettings.Key))
                throw new TokenException("Key hasn't been provided");

            var timeNow = DateTime.Now;
            var timeSpan = double.TryParse(_jwtTokenSettings.LifeSpan, out double time) ? time : defaultTime;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtTokenSettings.Key);
            var descriptor = new SecurityTokenDescriptor
            {
                NotBefore = timeNow,
                Expires = timeNow.AddMinutes(timeSpan),
                Audience = _jwtTokenSettings.Audiance,
                Issuer = _jwtTokenSettings.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
