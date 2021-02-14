using AuthenticationServerAPI.Models.Requests;
using AuthenticationServerAPI.Services.UserRepositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationServerAPI.Models;
using AuthenticationServerAPI.Services.PasswordHashes;
using AuthenticationServerAPI.Models.Responses;
using AuthenticationServerAPI.Services.TokenGenerators;
using AuthenticationServerAPI.Services.TokenValidators;
using AuthenticationServerAPI.Services.RefreshTokenRepositories;
using AuthenticationServerAPI.Services.Authenticators;

namespace AuthenticationServerAPI.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly Authenticator _authenticator;

        public AuthenticationController(IUserRepository userRepository, IPasswordHasher passwordHasher, AccessTokenGenerator accessTokenGenerator, RefreshTokenGenerator refreshTokenGenerator, RefreshTokenValidator refreshTokenValidator, IRefreshTokenRepository refreshTokenRepository, Authenticator authenticator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _refreshTokenValidator = refreshTokenValidator;
            _refreshTokenRepository = refreshTokenRepository;
            _authenticator = authenticator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestState();
            }

            if (registerRequest.Password != registerRequest.ConfirmPassword)
            {
                return BadRequest(new ErrorResponse("Password does not match confirm password"));
            }

            User existingUserByEmail = await _userRepository.GetByEmail(registerRequest.Email);
            if(existingUserByEmail != null)
            {
                return Conflict(new ErrorResponse("Email already exists"));
            }

            User existingUserByUsername = await _userRepository.GetByUsername(registerRequest.Username);
            if (existingUserByUsername != null)
            {
                return Conflict(new ErrorResponse("Username already exists"));
            }

            string passwordHash = _passwordHasher.HashPassword(registerRequest.Password);
            User registrationUser = new User()
            {
                Email = registerRequest.Email,
                Username = registerRequest.Username,
                PasswordHash = passwordHash
            };

            await _userRepository.Create(registrationUser);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestState();
            }

            User user = await _userRepository.GetByUsername(loginRequest.Username);
            if(user == null)
            {
                return Unauthorized();
            }

            bool isCorrectPassword = _passwordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash);
            if(!isCorrectPassword)
            {
                return Unauthorized();
            }

            AuthenticatedUserResponse response = await _authenticator.Authenticate(user);

            return Ok(response);
        }

        [HttpPost("request")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestState();
            }

            bool isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);
            if (!isValidRefreshToken)
            {
                return BadRequest(new ErrorResponse("Invalid refresh token"));
            }

            RefreshToken refreshTokenDTO = await _refreshTokenRepository.GetByToken(refreshRequest.RefreshToken);
            if(refreshTokenDTO == null)
            {
                return NotFound(new ErrorResponse("Invalid refresh token"));
            }

            _refreshTokenRepository.Delete(refreshTokenDTO.Id);

            User user = await _userRepository.GetById(refreshTokenDTO.UserId);
            if (user == null)
            {
                return NotFound(new ErrorResponse("User not found"));
            }

            AuthenticatedUserResponse response = await _authenticator.Authenticate(user);

            return Ok(response);
        }

        private IActionResult BadRequestState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(new ErrorResponse(errorMessages));
        }
    }
}
