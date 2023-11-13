using JWTAuthencation.Data;
using JWTAuthencation.Models;
using JWTAuthencation.Models.ViewModel;
using MessagePack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Twilio.Rest.Api.V2010.Account.Usage.Record;

namespace JWTAuthencation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RecommendController : ControllerBase
	{
		/*
         ANOMALIES
         - "Latitute", "Longtitute", "SexsualOrientation"
         - Nullable Ids, DateOfBirth
         */
		
		private readonly int maxAmt = 20;
		private readonly JWTAuthencationContext _context;
		public RecommendController(JWTAuthencationContext context) 
		{
			_context = context;
		}
		private bool CheckPair(int id, ConciseUser U)
		{
			ConciseUser conciseUser = CUser(id);
			int g = (conciseUser.Gender) ? 1 : 0;
			int ug = (U.Gender) ? 1 : 0;
			bool f1 = conciseUser.AgeMin <= U.Age;
			bool f2 = conciseUser.AgeMax >= U.Age;
			bool f3 = (conciseUser.SexO == 0 && U.SexO != 1 && (g + ug == 1) || //0 straight, 1 les-gay, 2 bi.
					  (conciseUser.SexO == 1 && U.SexO != 0 && (g + ug) % 2 == 0) ||
					  (conciseUser.SexO == 2 && U.SexO == 0 && g + ug == 1) ||
					  (conciseUser.SexO == 2 && U.SexO == 1 && (g + ug) % 2 == 0)) ||
					  (conciseUser.SexO == 2 && U.SexO == 2);
			bool f4 = false;
			foreach (int x in conciseUser.Language)
			{
				if (U.Language.Contains(x))
				{
					f4 = true;
					break;
				}
			}
			f4 = f4 || conciseUser.GlobalMatch;
			bool f5 = DistanceTo(id, U) <= (double)conciseUser.MaxDistance;
			return f1 && f2 && f3 && f4 && f5;
		}
		private double DistanceTo(int id, ConciseUser U)
		{
			ConciseUser conciseUser = CUser(id);
			var R = 3958.8; // Radius of the Earth in miles
			var rlat1 = conciseUser.Latitude * (Math.PI / 180);
			var rlat2 = U.Latitude * (Math.PI / 180);
			var difflat = rlat2 - rlat1;
			var difflon = (U.Longtitude - conciseUser.Longtitude) * (Math.PI / 180);
			var d = 2 * R * Math.Asin(Math.Sqrt(Math.Sin(difflat / 2) * Math.Sin(difflat / 2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Sin(difflon / 2) * Math.Sin(difflon / 2)));
			return d;
		}


		private bool CheckPairById(int id, ConciseUser U)
		{
			return CheckPair(id, U);
		}
		private ConciseUser CUser(int userId)
		{
			var user = _context.Users.FirstOrDefault(x => x.Id == userId);
			var userSetting = _context.Setting.FirstOrDefault(x => x.Id == userId);

			if (user == null || userSetting == null)
			{
				throw new Exception("Failed to read database.");
			}

			int Age = DateTime.Today.Year - user.DOB.Value.Year;

			if (user.DOB.Value.Date > DateTime.Today.AddYears(Age))
				Age--; //Age of partner 

			ConciseUser conciseUser = new ConciseUser()
			{
				Latitude = userSetting.Latitute.HasValue ? userSetting.Latitute.Value : 1.0,
				Longtitude = userSetting.Longtitute.HasValue ? userSetting.Longtitute.Value : 1.0,
				Age = Age,
				AgeMax = userSetting.AgeMax.HasValue ? userSetting.AgeMax.Value : 1, //Cann't null
				AgeMin = userSetting.AgeMin.HasValue ? userSetting.AgeMin.Value : 1, //Cann't null
				MaxDistance = userSetting.DistancePreference.HasValue ? userSetting.DistancePreference.Value * 621 : 1,
				SexO = user.SexsualOrientationID.HasValue ? user.SexsualOrientationID.Value : 2,
				GlobalMatch = userSetting.GlobalMatches.HasValue ? userSetting.GlobalMatches.Value == 1 : true,
				Language = _context.UsersLanguages
							.Where(x => x.Id == userId)
							.Select(x => x.LanguageId.HasValue ? x.LanguageId.Value : 0)
							.ToHashSet()
			};

			return conciseUser;
		}
		[HttpGet("RecommendList")]
		public async Task<IActionResult> RecommendList(int id)
		{
			ConciseUser user= CUser(id);

			var LikeIds = _context.Likes.Where(x => x.LikedUserId == id).Select(x => x.LikeUserId).OrderBy(x => Guid.NewGuid());
			var LikeIdList = LikeIds.ToList();
			var LikeIdSet = LikeIds.ToHashSet();
			var ListA = new List<int>();
			foreach (int lid in LikeIdList)
			{
				if (CheckPairById(lid, user))
					ListA.Add(lid);
				if (ListA.Count > maxAmt) break;
			}
			var UnlikeIdSet = _context.Unlike.Where(x => x.UnlikedUserId.Value == id).Select(x => x.UnlikeUserId).ToHashSet();
			var UnlikedIdSet = _context.Unlike.Where(x => x.UnlikeUserId.Value == id).Select(x => x.UnlikedUserId).ToHashSet();
			var ListB = _context.Users.Select(x => x.Id.Value).Where(x => !LikeIdSet.Contains(x) &&
																	 !UnlikedIdSet.Contains(x) &&
																	 !UnlikeIdSet.Contains(x) &&
																	 CheckPairById(x, user)).OrderBy(x => Guid.NewGuid()).ToList();
			return Ok(new Tuple<List<int>, List<int>>(ListA, ListB));
		}
	}
}
