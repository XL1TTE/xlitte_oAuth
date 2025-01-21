using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.EF_Repository;
using System.Text.Json;
using XLITTE_AuthorizationService.Services;

namespace XLITTE_AuthorizationService.Controllers.oauth
{
    [Route("users/")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersRepository _usersRepository;
        public UsersController(UsersRepository usersRepository)
        {
            _usersRepository = usersRepository;

        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(
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
                    var response = new
                    {
                        Login = NewUser.Login,
                        Email = NewUser.Email,
                        CreatedAt = NewUser.CreatedAt
                    };

                    string message = $"User: \n{JsonSerializer.Serialize(response)}\n was registered. Welcome {response.Login}!";
                    return Ok(message);
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
