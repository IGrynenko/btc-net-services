using UserService.Models;

namespace BTC.API.Models
{
    public class UserValidationSuccessResponse : UserDTO
    {
        public string Token { get; set; }

        public UserValidationSuccessResponse() { }

        public UserValidationSuccessResponse(UserDTO user)
        {
            Id = user.Id;
            Name = user.Name;
        }

        public UserValidationSuccessResponse(UserDTO user, string token) : this(user)
        {
            Token = token;
        }
    }
}
