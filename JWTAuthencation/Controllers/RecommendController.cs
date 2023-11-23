using JWTAuthencation.Data;
using JWTAuthencation.Models;
using JWTAuthencation.Models.ViewModel;
using MessagePack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.DiaSymReader;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Twilio.Rest.Api.V2010.Account.Usage.Record;


/* STD
- Required fields are ALL nullable, however DOB - Gender - Id MUST NOT equal null.
- ConciseUser fields are not nullable.
 * */
namespace JWTAuthencation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendController : ControllerBase
    {
        private readonly int maxAmt = 20;
        private readonly JWTAuthencationContext _context;
        public RecommendController(JWTAuthencationContext context) { _context = context; }

        private double HaversineDistance(double latA, double longA, double latB, double longB)
        {
            var R = 6371.0;
            var rlat1 = latA * (Math.PI / 180);
            var rlat2 = latB * (Math.PI / 180);
            var difflat = rlat2 - rlat1;
            var difflon = (longA - longB) * (Math.PI / 180);

            /*            2 * R * Math.Asin(
                            Math.Sqrt(
                                Math.Sin(difflat / 2) * Math.Sin(difflat / 2) + 
                                Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Sin(difflon / 2) * Math.Sin(difflon / 2)
                            )
                        )*/

            return 2 * R * Math.Asin(
                Math.Sqrt(
                    Math.Sin(difflat / 2) * Math.Sin(difflat / 2) +
                    Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Sin(difflon / 2) * Math.Sin(difflon / 2)
                )
            );
        }




        private ConciseUser ReadConciseUser(int userId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            var userSetting = _context.Setting.FirstOrDefault(x => x.Id == userId);
            if (user == null)
                throw new Exception("Failed to read user profile.");
            if (userSetting == null)
                throw new Exception("Failed to read user setting.");
            int Age = DateTime.Today.Year - (user.DOB ?? DateTime.Today).Year;
            int MaxDistance = (userSetting.DistancePreference ?? 1000000);
            if ((user.DOB ?? DateTime.Today) > DateTime.Today.AddYears(Age)) Age--;
            if ((userSetting.DistanceUnit ?? 1) == 2) MaxDistance *= 1000;
            return new ConciseUser()
            {
                Latitude = userSetting.Latitute ?? 0.0,
                Longtitude = userSetting.Longtitute ?? 0.0,
                Age = Age,
                AgeMax = userSetting.AgeMax ?? 100,
                AgeMin = userSetting.AgeMin ?? 0,
                MaxDistance = MaxDistance,
                SexO = user.SexsualOrientationID ?? 3, //1, 2, 3
                Global = userSetting.GlobalMatches ?? 1,
                Language = _context.UsersLanguages.Where(x => x.UserId == userId).Select(x => x.LanguageId ?? 0).ToHashSet()
            };
        }
        [HttpGet("RecommendList")]
        public async Task<IActionResult> RecommendList(int id) /*TWO TIMES FILTER METHOD.*/
        {
            ConciseUser user = ReadConciseUser(id);

            var LikeIdList = _context.Likes.Where(x => x.LikedUserId == id).Select(x => x.LikeUserId).OrderBy(x => Guid.NewGuid()).ToList(); //Raw candidates
            var ListA = new List<int>();
            var SetA = new HashSet<int>();
            foreach (int lid in LikeIdList) //One-way check: user favors liker
            {
                ConciseUser liker;
                try { liker = ReadConciseUser(lid); }
                catch { continue; }
                if (
                    (
                        (liker.Gender != user.Gender && liker.SexO != 2 && user.SexO != 2) ||
                        (liker.Gender == user.Gender && liker.SexO != 1 && user.SexO != 1)
                    ) &&
                    (liker.Age <= user.AgeMax && liker.Age >= user.AgeMin) &&
                    (HaversineDistance(user.Latitude, user.Longtitude, liker.Latitude, liker.Longtitude) <= user.MaxDistance)
                )
                {
                    if (user.Global == 1)
                    {
                        ListA.Add(lid);
                        SetA.Add(lid);
                    }
                    else foreach (int i in user.Language)
                        {
                            if (liker.Language.Contains(i))
                            {
                                ListA.Add(lid);
                                SetA.Add(lid);
                                break;
                            }
                        }
                }
                if (ListA.Count >= maxAmt)
                    break;
            }

            var UnlikeIdSet = _context.Unlike.Where(x => x.UnlikedUserId.Value == id).Select(x => x.UnlikeUserId.Value).ToHashSet();
            var UnlikedIdSet = _context.Unlike.Where(x => x.UnlikeUserId.Value == id).Select(x => x.UnlikedUserId.Value).ToHashSet();
            var FavorList = (
                from u in _context.Users
                join s in _context.Setting
                on u.SettingId equals s.Id
                join l in _context.UsersLanguages
                on u.Id equals l.UserId
                select new
                {
                    Id = u.Id.Value,
                    Gender = u.Gender ?? false,
                    SexO = u.SexsualOrientationID ?? 3,
                    AgeMax = s.AgeMax ?? 100,
                    AgeMin = s.AgeMin ?? 0,
                    DOB = u.DOB ?? DateTime.Today,
                    LanguageId = l.Id,
                    Global = s.GlobalMatches ?? 1,
                    Latitude = s.Latitute ?? 0,
                    Longtitude = s.Longtitute ?? 0,
                    MaxDistance = (s.DistancePreference ?? 1000000),
                    DistanceUnit = s.DistanceUnit ?? 1
                }
            ).Where //Twoway check, 1/2
            (
                x =>
                !SetA.Contains(x.Id) &&
                !UnlikeIdSet.Contains(x.Id) &&
                !UnlikedIdSet.Contains(x.Id) &&
                (
                    (x.Gender != user.Gender && x.SexO != 2 && user.SexO != 2) ||
                    (x.Gender == user.Gender && x.SexO != 1 && user.SexO != 1)
                ) &&
                (user.Age <= x.AgeMax && user.Age >= x.AgeMin) &&
                ((user.Global == 1 && x.Global == 1) || user.Language.Contains(x.LanguageId))
            ).GroupBy(x => x.Id).Select(x => x.First()).OrderBy(x => Guid.NewGuid()).ToList();
            var ListB = new List<int>();
            foreach (var x in FavorList) //Twoway check, 2/2
            {
                int xAge = DateTime.Today.Year - x.DOB.Year;
                int xMaxDistance = x.MaxDistance;
                double distance = HaversineDistance(user.Latitude, user.Longtitude, x.Latitude, x.Longtitude);
                if (x.DOB > DateTime.Today.AddYears(-xAge)) xAge--;
                if (x.DistanceUnit == 2) xMaxDistance *= 1000;
                if (xAge > user.AgeMax || xAge < user.AgeMin) continue;
                if (distance > Math.Min(user.MaxDistance, xMaxDistance)) continue;
                ListB.Add(x.Id);
                if (ListB.Count >= maxAmt)
                    break;
            }
            ListA.Remove(id); ListB.Remove(id);

            //Get all user match the user where has the id mention
            var matchUser = _context.Likes.
                Where(e => (e.LikeUserId == id || e.LikedUserId == id) && e.Matches == true)
                .Select(e => e.LikeUserId == id ? e.LikedUserId : e.LikeUserId)
                .ToList();
            ////Remove part 2
            ListA = ListA.Where(e => !matchUser.Contains(e)).ToList();
            ListB = ListB.Where(e => !matchUser.Contains(e)).ToList();

            //Remove part 3
            var likedUser = _context.Likes.
                    Where(e => e.LikeUserId == id)
                    .Select(e => e.LikeUserId == id ? e.LikedUserId : e.LikeUserId)
                    .ToList();
            ListA = ListA.Where(e => !likedUser.Contains(e)).ToList();
            ListB = ListB.Where(e => !likedUser.Contains(e)).ToList();

            //Remove part 4
            var unlikeUser = _context.Unlike.Where(e => e.UnlikeUserId == id).Select(e => e.UnlikedUserId).ToList();
            ListA = ListA.Where(e => !unlikeUser.Contains(e)).ToList();
            ListB = ListB.Where(e => !unlikeUser.Contains(e)).ToList();
            return Ok(new Tuple<List<int>, List<int>>(ListA, ListB));
        }

        [HttpPost]
        [Route("GetDetailRecommend")]
        public async Task<IActionResult> GetDetailRecommend(int userID, List<int> recommendList)
        {

            var User = _context.Setting.Where(e => e.Id == userID).Select(e => new { lat = e.Latitute, longt = e.Longtitute }).FirstOrDefault();
            var res = _context.Users.
                Where(e => recommendList.Contains(e.Id.Value)).
                Join(_context.Setting, u => u.Id, s => s.Id, (u, s) => new DetailRecommend
                {
                    ID = u.Id.Value,
                    FullName = u.FullName,
                    LiveAt = u.LiveAt,
                    Age = DateTime.UtcNow.Year - u.DOB.Value.Year - (DateTime.UtcNow < u.DOB.Value.AddYears(DateTime.UtcNow.Year - u.DOB.Value.Year) ? 1 : 0),
                    ImagePath = _context.Photo.Where(e => e.UserId == u.Id).Select(e => "https://localhost:7251/Uploads/" + e.ImagePath).ToList(),
                    Distance = Math.Round(6371.0 * 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin((s.Latitute.Value - User.lat.Value) * Math.PI / 180 / 2), 2) + Math.Cos(User.lat.Value * Math.PI / 180) * Math.Cos(s.Latitute.Value * Math.PI / 180) * Math.Pow(Math.Sin((s.Longtitute.Value - User.longt.Value) * Math.PI / 180 / 2), 2))), 1)
                });

            return Ok(res);
        }
        [HttpGet("RecommendInfo")]
        public async Task<IActionResult> RecommendInfo(int id)
        {
            ConciseUser user = ReadConciseUser(id);
            return Ok(user);
        }
    }
}
//1km 2m
