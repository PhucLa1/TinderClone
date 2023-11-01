using JWTAuthencation.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWTAuthencation.Models;
using JWTAuthencation.Models.OtherModels;
using System.Security.Cryptography;

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
                    JWTGenerator(Result);
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
        private dynamic JWTGenerator(User Result)
        {
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Id", Result.Id.ToString()),
                        new Claim("DisplayName", Result.FullName),
                        new Claim("UserName", Result.UserName),
                        new Claim("Password", Result.Pass),
                        //new Claim("Height", Result.Height.ToString())
                    };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: signIn);

            var encrypterToken = new JwtSecurityTokenHandler().WriteToken(token);

            SetJWT(encrypterToken);
            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken, Result);


            HttpContext.Response.Cookies.Append("token", encrypterToken,
                new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(7),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None
                });
            return new { token = encrypterToken, username = Result.UserName };
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };
            return refreshToken;
        }

        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshTOken = Request.Cookies["X-Refresh-Token"];

            var user = _context.Users.Where(e => e.Token == refreshTOken).FirstOrDefault();
            if(user == null || user.TokenExpires < DateTime.UtcNow)
            {
                return Unauthorized("Token has expired");
            }
            JWTGenerator(user);
            return Ok();
        }

        private void SetRefreshToken(RefreshToken refreshToken,User user)
        {
            HttpContext.Response.Cookies.Append("X-Refresh-Token", refreshToken.Token,
                new CookieOptions
               {
                    Expires = DateTime.UtcNow.AddDays(7),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None
                });
            var userOfRes = _context.Users.Where(e => e.UserName == user.UserName).FirstOrDefault();
            userOfRes.Token = refreshToken.Token;
            userOfRes.TokenCreated = refreshToken.Created;
            userOfRes.TokenExpires = refreshToken.Expires;
        }

        private void SetJWT(string encrypterToken)
        {
            HttpContext.Response.Cookies.Append("X-Access-Token", encrypterToken,
                 new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddMinutes(15),
                        HttpOnly = true,
                        Secure = true,
                        IsEssential = true,
                        SameSite = SameSiteMode.None
                     });
        }

        [HttpDelete]
        private async Task<IActionResult> RevokeToken(string username)
        {
            _context.Users.Where(e => e.UserName == username).FirstOrDefault().Token = string.Empty;
            return Ok();
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
                //Thêm người dùng
                _context.Users.Add(user);

                //Thêm setting
                Setting setting = new Setting() { };
                _context.Setting.Add(setting);

                //Thêm ngôn ngữ,sở thích
                List<Languages> languages = new List<Languages>();
                List<Passion> passions = new List<Passion>();
                Languages language = new Languages() { };
                Passion passion = new Passion() { };
                for (int i = 0; i < 5; i++)
                {
                    languages.Add(language);
                    passions.Add(passion);
                }
                _context.Languages.AddRange(languages);
                _context.Passion.AddRange(passions); 
                _context.SaveChanges();
                return Ok("Sign Up Successful");                              
            }
        }
    }
}
