using JWTAuthencation.Data;
using JWTAuthencation.Models;
using JWTAuthencation.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace JWTAuthencation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        private readonly JWTAuthencationContext _context;
        public DashBoardController(JWTAuthencationContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("GetCountMess")]
        public async Task<IActionResult> getCountMess(string date)
        {
            var res = _context.GetCountMess.FromSqlRaw("EXEC GetCountMess @Date", new SqlParameter("Date", date)).AsEnumerable().ToList();
            return Ok(res);
        }


        //Bieu do tron cho 16 bang con lai
        [HttpGet]
        [Route("GetSexOri")]
        public async Task<IActionResult> getSO()
        {
            var result = from user in _context.Users
                         join sexualOrientation in _context.SexsualOrientation
                         on user.SexsualOrientationID equals sexualOrientation.ID into joinedData
                         from joinData in joinedData.DefaultIfEmpty()
                         group new { user, joinData } by new { user.SexsualOrientationID, joinData.SOName } into groupedData
                         select new
                         {
                             SexsualOrientationID = groupedData.Key.SexsualOrientationID,
                             Name = groupedData.Key.SOName,
                             UserCount = groupedData.Count()
                         };
            return Ok(result.ToList());
        }

		[HttpGet]
		[Route("GetFutureFamily")]
		public async Task<IActionResult> getFF()
		{
			var result = from user in _context.Users
						 join futureFamily in _context.FutureFamily
						 on user.FutureFamilyID equals futureFamily.ID into joinedData
						 from joinData in joinedData.DefaultIfEmpty()
						 group new { user, joinData } by new { user.FutureFamilyID, joinData.FFName } into groupedData
						 select new
						 {
							 FutureFamilyID = groupedData.Key.FutureFamilyID,
							 Name = groupedData.Key.FFName,
							 UserCount = groupedData.Count()
						 };
			return Ok(result.ToList());
		}

		[HttpGet]
		[Route("GetVacxinCovid")]
		public async Task<IActionResult> getVC()
		{
			var result = from user in _context.Users
						 join vacxinCovid in _context.VacxinCovid
						 on user.VacxinCovidID equals vacxinCovid.ID into joinedData
						 from joinData in joinedData.DefaultIfEmpty()
						 group new { user, joinData } by new { user.VacxinCovidID, joinData.VCName } into groupedData
						 select new
						 {
							 VacxinCovidID = groupedData.Key.VacxinCovidID,
							 Name = groupedData.Key.VCName,
							 UserCount = groupedData.Count()
						 };
			return Ok(result.ToList());
		}
		[HttpGet]
		[Route("GetPurposeDate")]
		public async Task<IActionResult> getPP()
		{
			var result = from user in _context.Users
						 join purposeDate in _context.PurposeDate
						 on user.PurposeDateID equals purposeDate.ID into joinedData
						 from joinData in joinedData.DefaultIfEmpty()
						 group new { user, joinData } by new { user.PurposeDateID, joinData.PDName } into groupedData
						 select new
						 {
							 PurposeDateID = groupedData.Key.PurposeDateID,
							 Name = groupedData.Key.PDName,
							 UserCount = groupedData.Count()
						 };
			return Ok(result.ToList());
		}
		[HttpGet]
		[Route("GetEducation")]
		public async Task<IActionResult> getE()
		{
			var result = from user in _context.Users
						 join education in _context.Education
						 on user.EducationID equals education.ID into joinedData
						 from joinData in joinedData.DefaultIfEmpty()
						 group new { user, joinData } by new { user.EducationID, joinData.EName } into groupedData
						 select new
						 {
							 EducationID = groupedData.Key.EducationID,
							 Name = groupedData.Key.EName,
							 UserCount = groupedData.Count()
						 };
			return Ok(result.ToList());
		}

		[HttpGet]
		[Route("GetGender")]
		public async Task<IActionResult> getGender()
		{
			var result = _context.Users
				.GroupBy(e => e.Gender)
				.Select(g => new
				{
					Name = g.Key == false ? "Female" : "Male",
					UserCount = g.Count()
				})
				.ToList();
			return Ok(result);
		}

		[HttpGet]
		[Route("GetRegisDate")]
		public async Task<IActionResult> getRegisDate(DateTime startDate,DateTime endDate)
		{
			var res = _context.getCountUser.FromSqlRaw("EXEC getCountUser @startDate,@endDate", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate)).AsEnumerable().ToList();
			return Ok(res);
		}
	}
}
