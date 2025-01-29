using Domain;

using Microsoft.AspNetCore.Mvc;
using Persistence.EF_Repository;
using System.Text.Json;
using XLITTE_AuthorizationService.Models;
using XLITTE_AuthorizationService.Services;

namespace XLITTE_AuthorizationService.Controllers.oauth
{
    [Route("users/")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersRepository _usersRepository;
        private readonly string _secretKey = "3a2d289e-e044-4f98-9bfe-6ffb50e4d71b";
        public UsersController(UsersRepository usersRepository)
        {
            _usersRepository = usersRepository;

        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate(
            [FromBody] UserLoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                return BadRequest("Email and Password must be provided.");
            }
            User? user = await _usersRepository.GetByEmailAsync(loginRequest.Email);

            if (user == null || !PasswordSecurityService.VerifyPassword(loginRequest.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Unauthorized("Incorrect email or password.");
            }

            if (!Request.Headers.TryGetValue("Authorization", out var auth_token) || !auth_token.ToString().StartsWith("Bearer "))
            {
                return Unauthorized("Authorization token is missing or invalid.");
            }

            string bearer = auth_token.ToString().Split(' ')[1];
            JWTManager jwt = new JWTManager(bearer);

            if (!jwt.VerifyTokenSignature(_secretKey))
            {
                return Unauthorized("Invalid token signature.");
            }

            return Ok($"{bearer}: Authorized.");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] UserRegisterDto RegisterData)
        {
            (byte[] password_hash, byte[] salt) = PasswordSecurityService.GetPasswordHash(RegisterData.Password);

            User NewUser = new User
            {
                Login = RegisterData.Login,
                Email = RegisterData.Email,
                CreatedAt = DateTime.UtcNow,
                PasswordHash = password_hash,
                PasswordSalt = salt
            };
            try
            {
                if (await _usersRepository.AddAsync(NewUser))
                {
                    var Headers = new
                    {
                        alg = "HMAC256",
                        typ = "JWT",
                        iat = DateTime.UtcNow.ToLongTimeString(),
                        exp = DateTime.UtcNow.AddHours(1).ToLongTimeString(),
                    };
                    var Payload = new
                    {
                        UserId = NewUser.Id,
                        Username = NewUser.Login,
                        UserEmail = NewUser.Email,
                    };
                    string headers_json = JsonSerializer.Serialize(Headers);
                    string payload_json = JsonSerializer.Serialize(Payload);

                    string token = JWTManager.CreateToken(headers_json, payload_json, _secretKey);

                    var response = new
                    {
                        access_token = token,
                    };

                    return Ok(response);
                }
                return BadRequest();
            }
            catch
            {
                return StatusCode(500, "Server error.");
            }

        }
    }
}
