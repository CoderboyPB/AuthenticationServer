using AuthenticationServerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServerAPI.Services.UserRepositories
{
    public interface IUserRepository
    {
        Task<User> GetById(Guid id);
        Task<User> GetByEmail(string email);
        Task<User> GetByUsername(string username);
        Task<User> Create(User user);
    }
}
