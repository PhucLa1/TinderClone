using JWTAuthencation.Data;
using JWTAuthencation.Models;
using JWTAuthencation.Repositories;
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

        [Authorize]
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddNewUsers(Users user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok();
        }

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
        public async Task<IActionResult> Login(Users user)
        {
            if (user != null)
            {
                var resOfUser = _context.Users.Where(e => e.UserName == user.UserName && e.Pass == user.Pass).FirstOrDefault();
                if (resOfUser != null)
                {
                    user = resOfUser;
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
    }
}
