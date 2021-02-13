using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServerAPI.Models.Responses
{
    public class AuthenticatedUserResponse
    {
        public string AccessToken { get; set; }
    }
}
