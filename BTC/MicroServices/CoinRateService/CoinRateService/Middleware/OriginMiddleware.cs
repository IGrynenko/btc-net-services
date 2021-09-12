using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace CoinRateService.Middleware
{
    public class OriginMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _allowedHost;

        public OriginMiddleware(RequestDelegate next, string allowedHost)
        {
            _next = next;
            _allowedHost = allowedHost;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Host.HasValue && context.Request.Host.Host == _allowedHost)
            {
                await _next.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsJsonAsync(new { message = "Requests from this resource are not allowed" });
            }
        }
    }
}
