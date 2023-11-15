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
using Newtonsoft.Json;
using Google.Apis.Auth;

namespace JWTAuthencation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginSignUp : ControllerBase
    {
        private readonly JWTAuthencationContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;


        public LoginSignUp(
            JWTAuthencationContext context,
            IConfiguration configuration,
            HttpClient httpClient

            )
        {
            _context = context;
            _configuration = configuration;
            _httpClient = httpClient;

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
                    
                    return Ok(JWTGenerator(Result));
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
        [HttpGet]
        [Route("LoginAdminPage")]
        public async Task<IActionResult> LoginAdmin(string user,string pass)
        {

            if (user != null && pass != null)
            {
                var Result = _context.Admin.Where(e => e.UserName == user && e.Pass == pass).FirstOrDefault();
                if (Result != null)
                {
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        //new Claim("Id", Result.ID.ToString()),
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
                    SetRefreshTokenForAdmin(refreshToken, Result);

                    return Ok();
                }
                else
                {
                    return StatusCode(422,"Error data");
                }
            }
            else
            {
                return BadRequest("No data ");
            }
        }

        [HttpPost]
        [Route("FacebookLogin")]
        public async Task<IActionResult> FacebookLogin(string credential)
        {
            string appId = _configuration["Facebook:AppId"];
            string appSecret = _configuration["Facebook:AppSecret"];
            HttpResponseMessage debugTokenResponse = await _httpClient.GetAsync($"https://graph.facebook.com/debug_token?input_token={credential}&access_token={appId}|{appSecret}");
            var stringThing = await debugTokenResponse.Content.ReadAsStringAsync();
            var userOBJK = JsonConvert.DeserializeObject<FacebookTokenValidationResult>(stringThing);

            if (!userOBJK.Data.IsValid)
            {
                return Unauthorized("You are not logged into our app");
            }
            HttpResponseMessage meResponse = await _httpClient.GetAsync($"https://graph.facebook.com/me?fields=first_name,last_name,picture,email&access_token={credential}");
            var userContent = await meResponse.Content.ReadAsStringAsync();
            var userContentObj = JsonConvert.DeserializeObject<FacebookUserInfoResult>(userContent);

            var userSetting = _context.Setting.Where(e => e.Email == userContentObj.Email).FirstOrDefault();
            if(userSetting == null)
            {
                return NotFound("There are no accounts created by this facebook account");
            }
            else
            {
                var user = _context.Users.Where(e => e.SettingId == userSetting.Id).FirstOrDefault();
                return Ok(JWTGenerator(user));
            }
        }
        [HttpPost]
        [Route("GoogleLogin")]
        public async Task<IActionResult> GoogleLogin(string credential)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configuration["Google:ClientID"] }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);
            var userSetting = _context.Setting.Where(e => e.Email == payload.Email).FirstOrDefault();
            if(userSetting == null)
            {
                return BadRequest("No user use this email");
            }
            else
            {
                var user = _context.Users.Where(e => e.SettingId == userSetting.Id).FirstOrDefault();
                return Ok(JWTGenerator(user));
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

            return new { token = encrypterToken, username = Result.Id };
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
        private void SetRefreshTokenForAdmin(RefreshToken refreshToken, Admin admin)
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
            var adminOfRes = _context.Admin.Where(e => e.UserName == admin.UserName).FirstOrDefault();
            adminOfRes.Token = refreshToken.Token;
            adminOfRes.TokenCreated = refreshToken.Created;
            adminOfRes.TokenExpires = refreshToken.Expires;
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
        public async Task<IActionResult> RevokeToken(string username)
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
                return BadRequest("This account already exists");
            }
            else
            {
                int lastID = _context.Users.Count() + 1;
                //Thêm người dùng
                user.SettingId = lastID;
				_context.Users.Add(user);

                //Thêm setting
                Setting setting = new Setting() { };
                _context.Setting.Add(setting);

                //Thêm ngôn ngữ,sở thích
                List<UsersLanguages> languages = new List<UsersLanguages>();
                List<UsersPassion> passions = new List<UsersPassion>();
                UsersLanguages language = new UsersLanguages() { };
                UsersPassion passion = new UsersPassion() { };

                language.UserId = lastID;
                passion.UserId = lastID;

                for (int i = 0; i < 5; i++)
                {
                    languages.Add(language);
                    passions.Add(passion);
                }
                _context.UsersLanguages.AddRange(languages);
                _context.UsersPassion.AddRange(passions); 
                _context.SaveChanges();
                return Ok("Sign Up Successful");                              
            }
        }
    }
}
