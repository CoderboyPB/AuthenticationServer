using AuthenticationServerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServerAPI.Services.UserRepositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new List<User>();

        public Task<User> Create(User user)
        {
            user.Id = Guid.NewGuid();
            _users.Add(user);
            return Task.FromResult(user); //
        }

        public Task<User> GetByEmail(string email)
        {
           return Task.FromResult(_users.FirstOrDefault(u => u.Email == email));
        }

        public Task<User> GetById(Guid id)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
        }

        public Task<User> GetByUsername(string username)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Username == username));
        }
    }
}
