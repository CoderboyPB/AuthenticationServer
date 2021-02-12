using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServerAPI.Services.PasswordHashes
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
    }
}
