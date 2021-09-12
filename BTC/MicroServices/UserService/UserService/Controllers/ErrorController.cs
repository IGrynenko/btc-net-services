using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace UserService.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("error")]
        public object ErrorResponse()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            var code = context.Error is ArgumentException
                ? HttpStatusCode.BadRequest
                : HttpStatusCode.InternalServerError;

            Response.StatusCode = (int)code;

            return new { Message = context.Error.Message };
        }
    }
}
