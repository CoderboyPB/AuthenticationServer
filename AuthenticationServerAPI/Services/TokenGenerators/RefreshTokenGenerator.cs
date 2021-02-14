using AuthenticationServerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServerAPI.Services.TokenGenerators
{
    public class RefreshTokenGenerator
    {
        private readonly AuthenticationConfiguration _config;
        private readonly TokenGenerator _tokenGenerator;

        public RefreshTokenGenerator(AuthenticationConfiguration authenticationConfiguration, TokenGenerator tokenGenerator)
        {
            _config = authenticationConfiguration;
            _tokenGenerator = tokenGenerator;
        }

        public string GenerateToken()
        {
            return _tokenGenerator.GenerateToken(_config.RefreshTokenSecret, _config.Issuer, _config.Audience, _config.RefreshTokenExpirationMinutes);
        }
    }
}
