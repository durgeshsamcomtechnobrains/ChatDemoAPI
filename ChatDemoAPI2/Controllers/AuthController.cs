using ChatDemoAPI2.Model;
using ChatDemoAPI2.Model.Dtos;
using ChatDemoAPI2.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatDemoAPI2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private static ConcurrentDictionary<Guid, UserModel> LoggedInUsers = new ConcurrentDictionary<Guid, UserModel>();

        public AuthController(IUserService userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.RegisterUserAsync(model);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            var user = await Authenticate(userLogin);
            if (user != null)
            {
                var token = GenerateToken(user);
                var result = new ServiceResult
                {
                    Success = true,
                    Message = "Login successful",
                    Token = token,
                    UserId = user.Id,
                    Email = user.EmailAddress
                };

                //add user to logged-in users list
                LoggedInUsers[user.Id] = user;

                return Ok(result);
            }

            return Unauthorized(new ServiceResult { Success = false, Message = "Invalid username or password" });
        }

        [HttpGet("loggedinusers")]
        public IActionResult GetLoggedInUsers()
        {
            var users = LoggedInUsers.Values.ToList();
            return Ok(users);
        }

        [HttpGet("allusers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        private async Task<UserModel> Authenticate(UserLogin userLogin)
        {
            var user = await _userService.AuthenticateUserAsync(userLogin.Email, userLogin.Password);
            if (user != null)
            {
                return new UserModel { Id = user.Id, Username = user.Username, EmailAddress = user.Email };
            }

            return null;
        }

        private string GenerateToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.EmailAddress)
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
