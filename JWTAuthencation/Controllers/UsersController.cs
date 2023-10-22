using JWTAuthencation.Data;
using JWTAuthencation.HelpMethod;
using JWTAuthencation.Models;
using JWTAuthencation.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuthencation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly JWTAuthencationContext _context;
        private readonly IConfiguration _configuration;
        public UsersController(JWTAuthencationContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet]
        [Route("GetAllUser")]
        public async Task<ActionResult<IEnumerable<Users>>> getAll()
        {
            return _context.Users.ToList();
        }


        //[HttpPost]
        //[Route("Add")]
        //public async Task<IActionResult> AddNewUsers([FromForm]Users user)
        //{
        //    user.ImagePath = await HandleImage.Upload(user.FileImage);
        //    _context.Users.Add(user);
        //    _context.SaveChanges();
        //    return Ok();
        //}

        [Authorize]
        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IActionResult> UpdateUsers(int id, Users user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            return Ok();
        }

        [Authorize]
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteUsers(int id)
        {
            var user = _context.Users.Where(x => x.Id == id).FirstOrDefault();
            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("Details/{id}")]
        public async Task<ActionResult<Users>> GetUserById(int id)
        {
            var user = _context.Users.Where(x => x.Id == id).FirstOrDefault();
            return user;
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(string username,string pass)
        {
            if (username != null)
            {
                var user = _context.Users.Where(e => e.UserName == username && e.Pass == pass).FirstOrDefault();
                if (user != null)
                {
                    var claims = new[] {
                            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                            new Claim("Id", user.Id.ToString()),
                            new Claim("DisplayName", user.FullName),
                            new Claim("UserName", user.UserName),
                            new Claim("Password", user.Pass)
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
        [Route("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var res = await HandleImage.Upload(file);
            return Ok(res);
        }

        [HttpGet]
        [Route("GetImageUrl")]
        //Láy url của ảnh để hiện ra
        public async Task<IActionResult> GetImageUrl(string imagePath)
        {
            return await HandleImage.GetImageUrl(imagePath);
        }

        //[HttpGet("google-login")]
        //public async Task<IActionResult> GoogleLogin()
        //{
        //    // Thực hiện đăng nhập bên ngoài với Google
        //    var properties = new AuthenticationProperties { RedirectUri = "/api/auth/callback" };
        //    await HttpContext.ChallengeAsync("Google", properties);
        //}


        //[HttpGet("google-response")]
        //public async Task<IActionResult> GoogleResponse()
        //{
        //    var result = await HttpContext.AuthenticateAsync("Google");

        //    if (result.Succeeded)
        //    {
        //        // Xác thực thành công, tạo JWT Token và trả về cho người dùng.
        //        // Sử dụng thông tin từ result để tạo JWT Token.
        //        // Sau đó trả về token trong phản hồi.
        //        return Ok();
        //    }
        //    else
        //    {
        //        // Xác thực thất bại, xử lý lỗi tại đây.
        //        return BadRequest("Lỗi");
        //    }
        //}

    }
}
