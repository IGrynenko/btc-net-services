using System.ComponentModel.DataAnnotations;

namespace BTC.API.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(1, ErrorMessage = "Name can't be empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MinLength(1, ErrorMessage = "Password can't be empty")]
        public string Password { get; set; }
    }
}
