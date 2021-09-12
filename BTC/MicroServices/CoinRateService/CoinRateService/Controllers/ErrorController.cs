using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CoinRateService.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("error")]
        public object ErrorResponse()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return new { Message = context.Error.Message };
        }
    }
}
