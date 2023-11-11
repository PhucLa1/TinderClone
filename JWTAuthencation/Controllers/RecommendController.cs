//using JWTAuthencation.Data;
//using JWTAuthencation.Models;
//using MessagePack;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Twilio.Rest.Api.V2010.Account.Usage.Record;

//namespace JWTAuthencation.Controllers
//{
//	internal class ConciseUser
//	{
//		private readonly static JWTAuthencationContext _context = new JWTAuthencationContext();
//		private double Latitude { get; set; }
//		private double Longtitude { get; set; }
//		private int Age { get; set; }
//		private int AgeMax { get; set; }
//		private int AgeMin { get; set; }
//		private int MaxDistance { get; set; }
//		private bool Gender { get; set; }
//		private int SexO { get; set; }
//		private bool GlobalMatch { get; set; }
//		private HashSet<int> Language { get; set; }

//		public ConciseUser(int userId)
//		{
//			var user = _context.Users.Where(x => x.Id.Value == userId).FirstOrDefault();
//			var userSetting = _context.Setting.Where(x => x.Id == userId).FirstOrDefault();
//			if (user == null || userSetting == null)
//			{
//				throw new Exception("Failed to read database.");
//			}
//			this.Latitude = userSetting.Latitute.Value;
//			this.Longtitude = userSetting.Longtitute.Value;
//			this.Age = DateTime.Today.Year - user.DateOfBirth.Value.Year; //DateOfBirth must not be null. 
//			if (user.DateOfBirth.Value.Date > DateTime.Today.AddYears(-this.Age))
//				this.Age--; //Age of partner 
//			this.AgeMax = userSetting.AgeMax //Cann't null
//			this.AgeMin = userSetting.AgeMin //Cann't null

//			this.MaxDistance = this.MaxDistance * 621;
//			this.SexO = (user.SexsualOrientationID == null) ? 2 : user.SexsualOrientationID.Value;
//			this.GlobalMatch = (userSetting.GlobalMatches == null) ? true : (userSetting.GlobalMatches == 1);
//			this.Language = _context.UsersLanguages.Where(x => x.Id == userId).Select(x => x.LanguageId.Value).ToHashSet();
//		}
//		public double DistanceTo(ConciseUser U)
//		{
//			var R = 3958.8; // Radius of the Earth in miles
//			var rlat1 = this.Latitude * (Math.PI / 180);
//			var rlat2 = U.Latitude * (Math.PI / 180);
//			var difflat = rlat2 - rlat1;
//			var difflon = (U.Longtitude - this.Longtitude) * (Math.PI / 180);
//			var d = 2 * R * Math.Asin(Math.Sqrt(Math.Sin(difflat / 2) * Math.Sin(difflat / 2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Sin(difflon / 2) * Math.Sin(difflon / 2)));
//			return d;
//		}

//		public bool CheckPair(ConciseUser U)
//		{
//			int g = (this.Gender) ? 1 : 0;
//			int ug = (U.Gender) ? 1 : 0;
//			bool f1 = this.AgeMin <= U.Age;
//			bool f2 = this.AgeMax >= U.Age;
//			bool f3 = (this.SexO == 0 && U.SexO != 1 && (g + ug == 1) || //0 straight, 1 les-gay, 2 bi.
//					  (this.SexO == 1 && U.SexO != 0 && (g + ug) % 2 == 0) ||
//					  (this.SexO == 2 && U.SexO == 0 && g + ug == 1) ||
//					  (this.SexO == 2 && U.SexO == 1 && (g + ug) % 2 == 0)) ||
//					  (this.SexO == 2 && U.SexO == 2);
//			bool f4 = false;
//			foreach (int x in this.Language)
//			{
//				if (U.Language.Contains(x))
//				{
//					f4 = true;
//					break;
//				}
//			}
//			f4 = f4 || this.GlobalMatch;
//			bool f5 = this.DistanceTo(U) <= (double)this.MaxDistance;
//			return f1 && f2 && f3 && f4 && f5;
//		}

//		public bool CheckPairById(int userId)
//		{
//			return this.CheckPair(new ConciseUser(userId));
//		}
//	}

//	[Route("api/[controller]")]
//	[ApiController]
//	//[Authorize]
//	public class RecommendController : ControllerBase
//	{
//		/*
//         ANOMALIES
//         - "Latitute", "Longtitute", "SexsualOrientation"
//         - Nullable Ids, DateOfBirth
//         */
//		private readonly JWTAuthencationContext _context = new JWTAuthencationContext();
//		private readonly int maxAmt = 20;
//		[HttpGet("RecommendList")]
//		public async Task<IActionResult> RecommendList(int id)
//		{
//			ConciseUser user;
//			try
//			{
//				user = new ConciseUser(id);
//			}
//			catch
//			{
//				return BadRequest("Failed to read database.");
//			}
//			var LikeIds = _context.Like.Where(x => x.LikedUserId.Value == id).Select(x => x.LikeUserId).OrderBy(x => Guid.NewGuid());
//			var LikeIdList = LikeIds.ToList();
//			var LikeIdSet = LikeIds.ToHashSet();
//			var ListA = new List<int>();
//			foreach (int lid in LikeIdList)
//			{
//				if (user.CheckPairById(lid))
//					ListA.Add(lid);
//				if (ListA.Count > maxAmt) break;
//			}
//			var UnlikeIdSet = _context.Unlike.Where(x => x.UnlikedUserId.Value == id).Select(x => x.UnlikeUserId).ToHashSet();
//			var UnlikedIdSet = _context.Unlike.Where(x => x.UnlikeUserId.Value == id).Select(x => x.UnlikedUserId).ToHashSet();
//			var ListB = _context.Users.Select(x => x.Id.Value).Where(x => !LikeIdSet.Contains(x) &&
//																	 !UnlikedIdSet.Contains(x) &&
//																	 !UnlikeIdSet.Contains(x) &&
//																	 user.CheckPairById(x)).OrderBy(x => Guid.NewGuid()).ToList();
//			return Ok(new Tuple<List<int>, List<int>>(ListA, ListB));
//		}
//	}
//}
