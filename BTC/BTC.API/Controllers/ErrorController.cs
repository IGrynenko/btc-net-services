using BTC.API.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;

namespace BTC.API.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly IQueueService _queueService;

        public ErrorController(IQueueService queueService)
        {
            _queueService = queueService;
        }

        [Route("error")]
        public object ErrorResponse()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            _queueService.Publish(context.Error.Message);
            ((IDisposable)_queueService).Dispose();

            Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return new { Message = context.Error.Message };
        }
    }
}
