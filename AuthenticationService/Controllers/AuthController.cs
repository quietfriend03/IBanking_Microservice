using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthDBContext _userDbContext;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AuthController(AuthDBContext userDbContext, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _userDbContext = userDbContext;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return BadRequest("Invalid email or password.");
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var token_string = await generateTokenAsync(model);
                return Ok(token_string);
            }

            return BadRequest("Invalid email or password.");
        }



        [HttpPut("{userId}/{amount}")]
        public async Task<IActionResult> SubtractSodu(string userId, double amount)
        {
            // Find the user by UserId
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            if(user.Sodu < amount)
            {
                return BadRequest("Insufficient balance.");
            }
            user.Sodu -= amount;

            // Update the user
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok("Payment Completed");
            }
            else
            {
                return BadRequest("There something wrong happen");
            }
        }

        public async Task<string> generateTokenAsync(LoginModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.Username);
            var key = Encoding.UTF8.GetBytes("513D3993-A5C2-431A-8663-46B49CE6CD76");
            var id = user.Id;

            var claims = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, user.FullName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Sodu.ToString()),
                new Claim(JwtRegisteredClaimNames.Gender, user.PhoneNumber),
                new Claim("user_id", id),
            });;

            SigningCredentials signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.Now.AddMinutes(60),
                SigningCredentials = signingCredentials,
            };

            JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = securityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = securityTokenHandler.WriteToken(securityToken);
            return token;
        }
    }
}
