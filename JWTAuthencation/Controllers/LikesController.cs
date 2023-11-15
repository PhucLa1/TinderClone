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
				.ToList();

			var mess = _context.Mess
				.Where(e => e.SendUserId == userId || e.ReceiveUserId == userId)
				.Select(e => e.SendUserId == userId ? e.ReceiveUserId : e.SendUserId)
				.ToList();

			var users = _context.Users.ToList();
			var photos = _context.Photo.ToList();

			var uNMs = likes
				.Where(item => !mess.Contains(item.LikeUserId) && !mess.Contains(item.LikedUserId))
				.Select(item => new UserNotMess
				{
					UserID = item.LikeUserId == userId ? item.LikedUserId : item.LikeUserId,
					ImagePath = "https://localhost:7251/Uploads/" + photos.FirstOrDefault(e => e.UserId == item.LikeUserId)?.ImagePath,
					UserName = users.FirstOrDefault(e => e.Id == item.LikeUserId)?.UserName
				})
				.ToList();

			return Ok(uNMs);
		}
		[HttpGet]
		[Route("GetLikeMess")]
		public async Task<IActionResult> getLikeMess(int userId) //Get user not message
		{


			var LastMessages = _context.Mess
				.Where(e => e.SendUserId == userId || e.ReceiveUserId == userId)
				.GroupBy(e => e.SendUserId != userId ? e.SendUserId : e.ReceiveUserId)
				.Select(group => new
				{
					UserID = group.Key ,
					LastMessage = group.OrderByDescending(e => e.Id).Select(e => new {content= e.Content,sendId = e.SendUserId,sendTime = e.SendTime }).FirstOrDefault()
				})
				.ToList();


			var uMs = LastMessages.OrderByDescending(e => e.LastMessage.sendTime).Select(item => new UserMess
			{
				UserID = item.UserID,
				ImagePath = "https://localhost:7251/Uploads/"+ _context.Photo.Where(e => e.UserId == item.UserID).Select(e => e.ImagePath).FirstOrDefault(),
				UserName = _context.Users.Where(e => e.Id == item.UserID).Select(e => e.UserName).FirstOrDefault(),
				LastMess = LastMessages.Where(e => e.UserID == item.UserID).Select(e => e.LastMessage.content).FirstOrDefault(),
				LastUserChat = LastMessages.Where(e => e.UserID == item.UserID).Select(e => e.LastMessage.sendId).FirstOrDefault()
			}).ToList();

			return Ok(uMs);
		}

    }
}
