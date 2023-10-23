using JWTAuthencation.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWTAuthencation.Models;

namespace JWTAuthencation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginSignUp : ControllerBase
    {
        private readonly JWTAuthencationContext _context;
        private readonly IConfiguration _configuration;

        public LoginSignUp(JWTAuthencationContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(User user)
        {

            if (user != null)
            {
                var Result = _context.Users.Where(e => e.UserName == user.UserName && e.Pass == user.Pass).FirstOrDefault();
                if (Result != null)
                {

                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Id", Result.Id.ToString()),
                        //new Claim("DisplayName", Result.FullName),
                        new Claim("UserName", Result.UserName),
                        new Claim("Password", Result.Pass),
                        //new Claim("PhoneNumber", Result.PhoneNumber),
                        //new Claim("Height", Result.Height.ToString()),
                        //new Claim("Email", Result.Email)

                    };


                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);
                    user.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(user);
                }
                else
                {
                    return BadRequest("Error data");
                }
            }
            else
            {
                return BadRequest("No data ");
            }
        }


        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp(User user)
        {
            var findOfUser = _context.Users.Where(e => e.UserName == user.UserName).FirstOrDefault();
            if (findOfUser != null)
            {
                return Ok("This account already exists");
            }
            else
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return Ok("Sign Up Successful");
            }
        }
    }
}
