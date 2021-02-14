using AuthenticationServerAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServerAPI.Services.TokenGenerators
{
    public class AccessTokenGenerator
    {
        private readonly AuthenticationConfiguration _config;
        private readonly TokenGenerator _tokenGenerator;

        public AccessTokenGenerator(AuthenticationConfiguration authenticationConfiguration, TokenGenerator tokenGenerator)
        {
            _config = authenticationConfiguration;
            _tokenGenerator = tokenGenerator;
        }

        public string GenerateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            return _tokenGenerator.GenerateToken(_config.AccessTokenSecret, _config.Issuer, _config.Audience, _config.AccessTokenExpirationMinutes, claims);
        }
    }
}
