using Microsoft.AspNetCore.Builder;

namespace CoinRateService.Middleware
{
    public static class OriginExtention
    {
        public static IApplicationBuilder ValidateOrigin(this IApplicationBuilder builder, string allowedHost)
        {
            return builder.UseMiddleware<OriginMiddleware>(allowedHost);
        }
    }
}
