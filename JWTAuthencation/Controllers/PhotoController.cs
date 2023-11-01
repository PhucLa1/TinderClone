using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JWTAuthencation.Data;
using JWTAuthencation.Models;
using Microsoft.IdentityModel.Tokens;
using JWTAuthencation.HelpMethod;
using System.Net.NetworkInformation;

namespace JWTAuthencation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly JWTAuthencationContext _context;

        public PhotoController(JWTAuthencationContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var Photo = _context.Photo.ToList();
            if (Photo.IsNullOrEmpty())
            {
                return Ok("No data in the table");
            }
            return Ok(Photo);
        }
        [HttpGet]
        [Route("GetByID/{Id}")]
        public async Task<IActionResult> GetByID(int Id)
        {
            var result = _context.Photo.Where(e => e.Id == Id).FirstOrDefault();
            if (result == null)
            {
                return Ok("No data in the table");
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("AddNew")]
        public async Task<IActionResult> AddNew([FromForm] Photo photo)
        {
            var result = _context.Photo.Where(e => e.ImagePath == photo.ImagePath).FirstOrDefault();
            if (result == null)
            {
                string uploadedFileName = await HandleImage.Upload(photo.Image);
                var photopath = new Photo { UserId = photo.UserId };
                photopath.ImagePath = "Uploads/" + uploadedFileName;
                _context.Photo.Add(photopath);
                _context.SaveChanges();
                return Ok(photo);
            }
            return Ok("There is already exist name in table");
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Photo photo)
        {
            var result = _context.Photo.Where(e => e.Id == photo.Id).FirstOrDefault();
            _context.Photo.Update(result);
            _context.SaveChanges();
            if (result != null)
            {
                string path = photo.ImagePath;
                try
                {
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                catch
                {

                }
                _context.Photo.Remove(result);
                _context.SaveChanges();
                return Ok(result);
            }
            else
            {
                return Ok("There is haven't existed name in table");
            }
        }
    }
}