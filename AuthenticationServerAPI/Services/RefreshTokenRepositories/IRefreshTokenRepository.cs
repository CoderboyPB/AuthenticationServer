using AuthenticationServerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServerAPI.Services.RefreshTokenRepositories
{
    public interface IRefreshTokenRepository
    {
        Task Create(RefreshToken refreshToken);
        Task<RefreshToken> GetByToken(string token);
        Task Delete(Guid id);
    }
}
