using JWTAuthencation.Data;
using JWTAuthencation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthencation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SwipeController : ControllerBase
    {
        private readonly JWTAuthencationContext _context;
        public SwipeController(JWTAuthencationContext context) { _context = context; }
        [HttpGet]
        [Route("Like")]
        public async Task<IActionResult> Likes(int LikeID,int LikedID)
        {
            var res = _context.Likes.Where(e => e.LikedUserId == LikeID && e.LikeUserId == LikedID).FirstOrDefault();
            //Day la thang like dau tien
            if(res == null)
            {
                Like like = new Like()
                {
                    LikeUserId = LikeID,
                    LikedUserId = LikedID,
                    LikeDate = DateTime.UtcNow,
                    Matches = false
                };
                _context.Likes.Add(like);
                
            }
            else
            {
                res.Matches = true;
                res.LikeDate = DateTime.UtcNow;
                _context.Likes.Update(res);
            }
            _context.SaveChanges();
            return Ok("Like Successfully");
        }

        [HttpGet]
        [Route("Unlike")]
        public async Task<IActionResult> Unlikes(int unlikeID,int unlikedID)
        {
            Unlike unlike = new Unlike()
            {
                UnlikeUserId = unlikeID,
                UnlikedUserId = unlikedID,
                UnlikeDate = DateTime.UtcNow,
            };
            _context.Unlike.Add(unlike);
            _context.SaveChanges();
            return Ok("Unlike Successfully");
        }
    }
}
