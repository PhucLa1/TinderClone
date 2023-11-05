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
using System;

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
				.Join(_context.Passion, up => up.PassionId, p => p.Id, (up, p) => p.Pname).ToList();
			var languages = _context.UsersLanguages.Where(ul => ul.UserId == id)
				.Join(_context.Languages, ul => ul.LanguageId, l => l.Id, (ul, l) => l.Lname).ToList();

			//Tạo đường dẫn đến ảnh
			string protocol = Request.Scheme;
			string host = Request.Host.ToString();
			string url = protocol + "://" + host;
			//Chưa cần dùng ngay
			var photos = _context.Photo.Where(e => e.UserId == id).Select(e => "https://localhost:7251/Uploads/" + e.ImagePath).ToList();
			UserInfo userProfile = _context.
				GetUserProfile.FromSqlRaw("EXEC GetUserProfile @userID", new SqlParameter("userId", id))
				.AsEnumerable().FirstOrDefault();
			userProfile.passion = passion;
			userProfile.languages = languages;
			userProfile.photos = photos;
			return Ok(userProfile);
		}

		[HttpGet]
		[Route("GetAllUser")]
		public async Task<IActionResult> getAllUser()
		{
			var passions = _context.UsersPassion
				.Join(_context.Passion, up => up.PassionId, p => p.Id, (up, p) => new { UsersPassion = up, Passion = p })
				.Select(joined => new
				{
					UserId = joined.UsersPassion.UserId,
					Pname = joined.Passion.Pname
				})
				.ToList();
			var languages = _context.UsersLanguages
				.Join(_context.Languages, ul => ul.LanguageId, l => l.Id, (ul, l) => new { UsersLanguages = ul, Languages = l })
				.Select(joined => new
				{
					UserId = joined.UsersLanguages.UserId,
					Lname = joined.Languages.Lname
				}).ToList();

			//Tạo đường dẫn đến ảnh
			string protocol = Request.Scheme;
			string host = Request.Host.ToString();
			string url = protocol + "://" + host;
			//Chưa cần dùng ngay
			var photos = _context.Photo.Select(e => new
			{
				ImagePath = "https://localhost:7251/Uploads/" + e.ImagePath,
				UserId = e.UserId
			}).ToList();

			var userProfile = _context.
				GetAllUserProfile.FromSqlRaw("EXEC GetAllUserProfile")
				.AsEnumerable().ToList();
			foreach( var user in userProfile)
			{
				var passion = passions.Where(e => e.UserId == user.ID).Select(e => e.Pname).ToList();
				var language = languages.Where(e => e.UserId == user.ID).Select(e => e.Lname).ToList();
				var photo = photos.Where(e => e.UserId == user.ID).Select(e => e.ImagePath).ToList();

				//Gán vào các user
				user.passion = passion;
				user.languages = language;
				user.photos = photo;
			}

			return Ok(userProfile);
		}

		[HttpPost]
		[Route("UploadFile")]
		public async Task<IActionResult> UploadFile(IFormFile file)
		{
			var res = await HandleImage.Upload(file);
			return Ok(res);
		}

		[HttpPost]
		[Route("GenAutomaticallyUser")]
		public async Task<IActionResult> GenAutoUser(int numberOfUser)
		{
			//Khoi tao doi tuong Random
			Random random = new Random();

			List<User> users = new List<User>();
			List<Setting> settings = new List<Setting>();
			List<UsersLanguages> usersLanguages = new List<UsersLanguages>();
			List<UsersPassion> usersPassions = new List<UsersPassion>();
			List<Photo> photos = new List<Photo>();

			//Lấy số lượng các bản ghi của các bảng
			int countSO = _context.SexsualOrientation.Count();
			int countC = _context.Communication.Count();
			int countLL = _context.LoveLanguage.Count();
			int countPet = _context.Pet.Count();
			int countAlcohol = _context.Alcolhol.Count();
			int countDiet = _context.Diet.Count();
			int countVC = _context.VacxinCovid.Count();
			int countZodiac = _context.Zodiac.Count();
			int countPersonality = _context.Personality.Count();
			int countSmoke = _context.Smoke.Count();
			int countSM = _context.SocialMedia.Count();
			int countEducation = _context.Education.Count();
			int countPP = _context.PurposeDate.Count();
			int countFF = _context.FutureFamily.Count();
			int countWorkout = _context.Workout.Count();
			int countSH = _context.SleepHabit.Count();
			int countPassions = _context.Passion.Count();
			int countLanuages = _context.Languages.Count();
			int countAboutUser = HandleVirtualData.aboutUsers.Count();
			int countInforLogin = HandleVirtualData.names.Count();

			int lastID = _context.Users.Count() + 1;


			for (int i = 1; i <= numberOfUser; i++)
			{

				//Khoi tao Setting va User
				Setting setting = new Setting();
				User user = new User();

				//Xử lí phần gen mã ảo
				short genCountSO = (short)random.Next(countSO + 1);
				short genCountC = (short)random.Next(countC + 1);
				short genCountLL = (short)random.Next(countLL + 1);
				short genCountPet = (short)random.Next(countPet + 1);
				short genCountAlcohol = (short)random.Next(countAlcohol + 1);
				short genCountDiet = (short)random.Next(countDiet + 1);
				short genCountVC = (short)random.Next(countVC + 1);
				short genCountZodiac = (short)random.Next(countZodiac + 1);
				short genCountPersonality = (short)random.Next(countPersonality + 1);
				short genCountSmoke = (short)random.Next(countSmoke + 1);
				short genCountSM = (short)random.Next(countSM + 1);
				short genCountEducation = (short)random.Next(countEducation + 1);
				short genCountPP = (short)random.Next(countPP + 1);
				short genCountFF = (short)random.Next(countFF + 1);
				short genCountWorkout = (short)random.Next(countWorkout + 1);
				short genCountSH = (short)random.Next(countSH + 1);
				short genCountPermis = (short)random.Next(1, 4);
				bool gender = random.Next(2) == 0;
				int aboutUser = random.Next(countAboutUser);
				int genNames = random.Next(countInforLogin);
				int genPass = random.Next(countInforLogin);

				//Xử lí thêm dữ liệu cho Users
				user.SexsualOrientationID = genCountSO == 0 ? null : genCountSO;
				user.CommunicationID = genCountC == 0 ? null : genCountC;
				user.LoveLanguageID = genCountLL == 0 ? null : genCountLL;
				user.PetID = genCountPet == 0 ? null : genCountPet;
				user.AlcolholID = genCountAlcohol == 0 ? null : genCountAlcohol;
				user.DietID = genCountDiet == 0 ? null : genCountDiet;
				user.VacxinCovidID = genCountVC == 0 ? null : genCountVC;
				user.ZodiacID = genCountZodiac == 0 ? null : genCountZodiac;
				user.PersonalityID = genCountPersonality == 0 ? null : genCountPersonality;
				user.SmokeID = genCountSmoke == 0 ? null : genCountSmoke;
				user.SocialMediaID = genCountSM == 0 ? null : genCountSM;
				user.EducationID = genCountEducation == 0 ? null : genCountEducation;
				user.PurposeDateID = genCountPP == 0 ? null : genCountPP;
				user.FutureFamilyID = genCountFF == 0 ? null : genCountFF;
				user.WorkoutID = genCountWorkout == 0 ? null : genCountWorkout;
				user.SleepHabitID = genCountSH == 0 ? null : genCountSH;
				user.PermissionId = genCountPermis;
				user.Gender = gender;
				user.SettingId = lastID;
				user.AboutUser = HandleVirtualData.aboutUsers[aboutUser];
				user.FullName = HandleVirtualData.names[genNames];
				user.UserName = HandleVirtualData.usernames[genNames];
				user.Pass = HandleVirtualData.passwords[genPass];

				//Them du lieu vao DB
				users.Add(user);
				settings.Add(setting);

				//Them Passion va Languages

				for (int j = 1; j <= 5; j++)
				{
					short genCountPassions = (short)random.Next(countPassions + 1);
					short genCountLanguages = (short)random.Next(countLanuages + 1);


					UsersLanguages uL = new UsersLanguages();
					UsersPassion uP = new UsersPassion();
					uL.UserId = lastID;
					uL.LanguageId = genCountLanguages == 0 ? null : genCountLanguages;
					uP.UserId = lastID;
					uP.PassionId = genCountPassions == 0 ? null : genCountPassions;

					//Them vao List
					usersLanguages.Add(uL);
					usersPassions.Add(uP);

				}


				int countVirtualImages = 30;
				int genCountImages = random.Next(1, 9);
				for (int j = 1; j <= genCountImages; j++)
				{
					Photo photo = new Photo();
					photo.UserId = lastID;
					int image = random.Next(1, countVirtualImages); //Lấy số ảnh từ 1 đến 30
					string imagePath = "";
					if (gender == true) //Tức là nam
					{
						imagePath = $"Boys/{image}.jpg";
					}
					else
					{
						imagePath = $"Girls/{image}.jpg";
					}
					photo.ImagePath = imagePath;
					photos.Add(photo);
				}
				//Cong them ID vao
				lastID++;
			}

			_context.Users.AddRange(users);
			_context.Setting.AddRange(settings);
			_context.UsersLanguages.AddRange(usersLanguages);
			_context.UsersPassion.AddRange(usersPassions);
			_context.Photo.AddRange(photos);
			_context.SaveChanges();


			return Ok("Add successfully " + numberOfUser + " user");
		}


	}
}
