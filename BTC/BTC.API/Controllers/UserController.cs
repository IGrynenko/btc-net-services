using BTC.API.Interfaces;
using BTC.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;
using UserService.Models;

namespace BTC.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRestClient _client;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public UserController(IRestClient client, ITokenService tokenService, IConfiguration configuration)
        {
            _client = client;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [HttpGet("test")]
        public object Test()
        {
            throw new Exception("TEST");
        }

        [HttpPost("create")]
        public async Task<ActionResult<UserDTO>> SignupUser([FromBody] UserModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _client.BaseUrl = new Uri(_configuration.GetSection("Services:UserService:Url").Value);
            var request = new RestRequest(_configuration.GetSection("Services:UserService:Login").Value, Method.GET);
            var result = await _client.GetAsync<UserDTO>(request);

            if (result != null && result is IRestResponse response)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                    return Ok(result);
                else
                    return GetHttpErrorResponses(response.StatusCode);
            }
            else
                return StatusCode((int)HttpStatusCode.InternalServerError, "User service is not available");
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserValidationSuccessResponse>> ValidateUser([FromBody] UserModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _client.BaseUrl = new Uri(_configuration.GetSection("Services:UserService:Url").Value);
            var request = new RestRequest(_configuration.GetSection("Services:UserService:Login").Value, Method.GET);
            var result = await _client.GetAsync<UserDTO>(request);

            if (result != null && result is IRestResponse response)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var token = _tokenService.GenerateJwtToken();
                    return Ok(new UserValidationSuccessResponse(result, token));
                }
                else
                    return GetHttpErrorResponses(response.StatusCode);                
            }
            else
                return StatusCode((int)HttpStatusCode.InternalServerError, "User service is not available");
        }

        private ObjectResult GetHttpErrorResponses(HttpStatusCode code)
        {
            switch (code)
            {
                case HttpStatusCode.BadRequest:
                    return Conflict(new { Message = "User already exists" });
                case HttpStatusCode.NotFound:
                    return NotFound(new { Message = "User doesn't exist" });
                default:
                    return StatusCode((int)HttpStatusCode.InternalServerError, "User service is not available");
            }
        }
    }
}
