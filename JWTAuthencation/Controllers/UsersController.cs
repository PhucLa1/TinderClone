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
using JWTAuthencation.Models.ViewModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthencation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly JWTAuthencationContext _context;
        public UsersController(JWTAuthencationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetDetailUser")]
        public async Task<IActionResult> getDetail(int id)
        {
            var passion = _context.UsersPassion.Where(up => up.UserId == id)
                .Join(_context.Passion,up => up.PassionId,p => p.Id,(up, p) => p.Pname).ToList();
            var languages = _context.UsersLanguages.Where(ul => ul.UserId == id)
                .Join(_context.Languages, ul => ul.LanguageId, l => l.Id, (ul, l) => l.Lname).ToList();
            var photos = _context.Photo.Select(e => "https://localhost:7251/Uploads/"+e.ImagePath).ToList();
            UserInfo userProfile = _context.
                GetUserProfile.FromSqlRaw("EXEC GetUserProfile @userID", new SqlParameter("userId", id))
                .AsEnumerable().FirstOrDefault();
            userProfile.passion = passion;
            userProfile.languages = languages;
            userProfile.photos = photos;
            return Ok(userProfile);

        }

        [HttpPost]
        [Route("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var res = await HandleImage.Upload(file);
            return Ok(res);
        }

      
    }
}
