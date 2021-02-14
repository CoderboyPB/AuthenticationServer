using AuthenticationServerAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServerAPI.Services.TokenValidators
{
    public class RefreshTokenValidator
    {
        private readonly AuthenticationConfiguration authenticationConfiguration;

        public RefreshTokenValidator(AuthenticationConfiguration authenticationConfiguration)
        {
            this.authenticationConfiguration = authenticationConfiguration;
        }

        public bool Validate(string refreshToken)
        {
            TokenValidationParameters tokenValidationParameters= new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.RefreshTokenSecret)),
                ValidIssuer = authenticationConfiguration.Issuer,
                ValidAudience = authenticationConfiguration.Audience,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
