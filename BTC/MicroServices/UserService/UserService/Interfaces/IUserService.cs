using UserService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserService.Interfaces
{
    public interface IUserService
    {
        Task<User> AddUser(UserModel model);
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(UserModel model);
    }
}