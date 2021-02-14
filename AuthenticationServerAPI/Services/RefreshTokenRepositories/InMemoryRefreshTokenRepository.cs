using AuthenticationServerAPI.Models;
using AuthenticationServerAPI.Services.UserRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServerAPI.Services.RefreshTokenRepositories
{
    public class InMemoryRefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly List<RefreshToken> refreshTokens = new List<RefreshToken>();

        public Task Create(RefreshToken refreshToken)
        {
            refreshToken.Id = Guid.NewGuid();

            refreshTokens.Add(refreshToken);

            return Task.CompletedTask;
        }

        public Task Delete(Guid id)
        {
            refreshTokens.RemoveAll(t => t.Id == id);
            return Task.CompletedTask;
        }

        public Task<RefreshToken> GetByToken(string token)
        {
            var refreshToken = refreshTokens.FirstOrDefault(rt => rt.Token == token);
            return Task.FromResult(refreshToken);
        }
    }
}
