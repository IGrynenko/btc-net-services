using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserService.Interfaces;
using UserService.Models;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<ActionResult<UserDTO>> SignupUser([FromBody] UserModel model)
        {
            var user = await _userService.AddUser(model);

            if (user == null)
                return Conflict(new { Message = "User already exists" });

            return Ok(_mapper.Map<UserDTO>(user));
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> ValidateUser([FromBody] UserModel model)
        {
            var user = await _userService.GetUser(model);

            if (user != null)
                return Ok(_mapper.Map<UserDTO>(user));

            else
                return NotFound("User doesn't exist");
        }
    }
}
