using JWTAuthencation.Data;
using JWTAuthencation.HelpMethod;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly JWTAuthencationContext _context;
        public UsersController(JWTAuthencationContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        [Route("GetAllUser")]
        public async Task<ActionResult<IEnumerable<User>>> getAll()
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
        public async Task<IActionResult> UpdateUsers(int id, User user)
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
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = _context.Users.Where(x => x.Id == id).FirstOrDefault();
            return user;
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
    }
}
