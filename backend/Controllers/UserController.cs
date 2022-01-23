using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationSettings _appSettings;

        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IOptions<ApplicationSettings> appSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        [Route("Register")]
        // POST : /api/
        public async Task<Object> Register(IdentityUserModel model)
        {
            var applicationUser = new IdentityUser()
            {
                UserName = model.UserName,
                Email = model.Email
            };

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, model.Password);
                return Ok(result);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        [HttpPost]
        [Route("Login")]
        // POST : /api/User/Login
        public async Task<IActionResult> Login(LoginModel model)
        {
            var userData = await _userManager.FindByNameAsync(model.UserName);
            var passwordIsCorrect = await _userManager.CheckPasswordAsync(userData, model.Password);

            if (userData != null && passwordIsCorrect)
            {
                var settings = _appSettings.JWT_Secret;

                var role = await _userManager.GetRolesAsync(userData);

                var key = Encoding.UTF8.GetBytes(_appSettings.JWT_Secret);
                var expires = DateTime.UtcNow.AddDays(5);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserId", userData.Id.ToString())
                    }),
                    Expires = expires,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);

                return Ok(new { token, userData.UserName, userData.Email, expires, role });
            }
            else
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
        }
    }
}
