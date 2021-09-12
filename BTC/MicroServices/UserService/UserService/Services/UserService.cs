using AutoMapper;
using UserService.Interfaces;
using UserService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace UserService.Services
{
    public class UserService : IUserService
    {
        private readonly IDataWorker<User> _dataWorker;
        private readonly IMapper _mapper;

        public UserService(IDataWorker<User> dataWorker, IMapper mapper)
        {
            _dataWorker = dataWorker;
            _mapper = mapper;
        }

        public async Task<User> AddUser(UserModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Password))
                throw new ArgumentException();

            var existingUser = await GetUser(model);

            if (existingUser != null)
                return null;

            var user = _mapper.Map<User>(model);
            _dataWorker.WriteTable(user);

            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await Task.Run(() =>
            {
                IEnumerable<User> allUsers = null;
                try
                {
                    allUsers = _dataWorker.ReadTable();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw;
                }
                return allUsers;
            });
        }

        public async Task<User> GetUser(UserModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Password))
                throw new ArgumentException();

            var users = await GetUsers();

            return users.FirstOrDefault(e => e.Name.Equals(model.Name) && e.Password.Equals(model.Password));
        }
    }
}
