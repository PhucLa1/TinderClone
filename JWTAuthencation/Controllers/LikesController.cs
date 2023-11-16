using JWTAuthencation.Data;
using JWTAuthencation.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace JWTAuthencation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LikesController : ControllerBase
	{
		private readonly JWTAuthencationContext _context;
		public LikesController(JWTAuthencationContext context)
		{
			_context = context;
		}
		[HttpGet]
		[Route("GetLikeNotMess")]
		public async Task<IActionResult> getLikeNotMess(int userId) //Get user not message
		{
			var likes = _context.Likes
				.Where(e => (e.LikeUserId == userId || e.LikedUserId == userId) && e.Matches == true)
				.OrderByDescending(e => e.LikeDate)
				.Select(e => e.LikeUserId == userId ? e.LikedUserId : e.LikeUserId)
				.ToList();

			var mess = _context.Mess
				.Where(e => e.SendUserId == userId || e.ReceiveUserId == userId)
                .GroupBy(e => e.SendUserId == userId ? e.ReceiveUserId : e.SendUserId)
				.Select(e =>  e.Key )
                .ToList();
			var uNMs = likes.Where(e => !mess.Contains(e)).Select(e => new UserNotMess
			{
				UserID = e,
				ImagePath = "https://localhost:7251/Uploads/" + _context.Photo.Where(x => x.UserId == e).Select(e => e.ImagePath).FirstOrDefault(),
				UserName = _context.Users.Where(x => x.Id == e).Select(e => e.UserName).FirstOrDefault()
			}).ToList();
            return Ok(uNMs);
		}
		[HttpGet]
		[Route("GetLikeMess")]
		public async Task<IActionResult> getLikeMess(int userId) //Get user not message
		{
			var LastMessages = _context.Mess
				.Where(e => e.SendUserId == userId || e.ReceiveUserId == userId)
                .OrderByDescending(e => e.SendTime)
                .GroupBy(e => e.SendUserId != userId ? e.SendUserId : e.ReceiveUserId)				
				.ToList();


			//var uMs = LastMessages.OrderByDescending(e => e.LastMessage.sendTime).Select(item => new UserMess
			//{
			//	UserID = item.UserID,
			//	ImagePath = "https://localhost:7251/Uploads/"+ _context.Photo.Where(e => e.UserId == item.UserID).Select(e => e.ImagePath).FirstOrDefault(),
			//	UserName = _context.Users.Where(e => e.Id == item.UserID).Select(e => e.UserName).FirstOrDefault(),
			//	LastMess = LastMessages.Where(e => e.UserID == item.UserID).Select(e => e.LastMessage.content).FirstOrDefault(),
			//	LastUserChat = LastMessages.Where(e => e.UserID == item.UserID).Select(e => e.LastMessage.sendId).FirstOrDefault()
			//}).ToList();

			return Ok(LastMessages);
		}

    }
}
