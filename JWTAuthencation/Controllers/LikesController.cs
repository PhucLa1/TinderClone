using JWTAuthencation.Data;
using JWTAuthencation.Models;
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
		public async Task<IActionResult> getLikeMess(int userId) //Get user message
		{
			var LastMessages = _context.Mess
				.Where(e => e.SendUserId == userId || e.ReceiveUserId == userId)
				.OrderByDescending(e => e.Id)
                .GroupBy(e => e.SendUserId != userId ? e.SendUserId : e.ReceiveUserId)
				.Select(group => group.OrderByDescending(e => e.SendTime).FirstOrDefault())
                .ToList();
			var res = LastMessages
				.OrderByDescending(e => e.Id)
				.Select(e => new
				{
					UserID = e.SendUserId != userId ? e.SendUserId : e.ReceiveUserId,
					LastMessages = e.Content,
					LastUserChat = e.SendUserId,
					SendTime = e.SendTime
				})
				.Select(e => new UserMess
				{
					UserID = e.UserID,
					ImagePath = "https://localhost:7251/Uploads/" + _context.Photo.Where(x => x.UserId == e.UserID).Select(e => e.ImagePath).FirstOrDefault(),
					UserName = _context.Users.Where(x => x.Id == e.UserID).Select(e => e.UserName).FirstOrDefault(),
					LastMess = e.LastMessages,
					LastUserChat = e.LastUserChat,
					SendTime = e.SendTime
				}).ToList();
			return Ok(res);
		}
		[HttpGet]
		[Route("GetTitleForMess")]
		public async Task<IActionResult> GetTitleForMess(int fromId, int toId)
		{
			var res = _context.Likes
				.Where(e => (e.LikeUserId == fromId && e.LikedUserId == toId) || (e.LikeUserId == toId && e.LikedUserId == fromId))
				.Select(e => new
				{
					Image = _context.Photo.Where(e => e.UserId == toId).Select(e => "https://localhost:7251/Uploads/" + e.ImagePath).FirstOrDefault(),
					Name = _context.Users.Where(e => e.Id == toId).Select(e => e.FullName).FirstOrDefault(),
					DayMatch = e.LikeDate
				})
				.FirstOrDefault();
			return Ok(res);
		}
		[HttpGet]
		[Route("GetCountLike")]
		public async Task<IActionResult> GetCountLike(int userId)
		{
			var res = _context.Likes.Where(e => e.LikedUserId == userId && e.Matches == false).ToList().Count();
			return Ok(res);
		}


    }
}
