using JWTAuthencation.Data;
using JWTAuthencation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthencation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class VacxinCovidController : ControllerBase
	{
		private readonly JWTAuthencationContext _context;
		public VacxinCovidController(JWTAuthencationContext context)
		{
			_context = context;
		}
		[HttpGet]
		[Route("GetAll")]
		public async Task<IActionResult> getAll()
		{
			var res = _context.VacxinCovid.ToList();
			if (res.IsNullOrEmpty())
			{
				return NotFound("No data in the table");
			}
			return Ok(res);
		}
		[HttpGet]
		[Route("GetByID")]
		public async Task<IActionResult> getByID(int ID)
		{
			var res = _context.VacxinCovid.Where(e => e.ID == ID && e.OfStatus == 1).FirstOrDefault();
			if (res == null)
			{
				return NotFound("Can find by ID");
			}
			return Ok(res);
		}
		[HttpGet]
		[Route("GetByName")]
		public async Task<IActionResult> getByName(string Name)
		{
			var res = _context.VacxinCovid.Where(e => e.VCName.Trim().ToLower().Contains(Name.Trim().ToLower())
											&& e.OfStatus == 1).ToList();
			if (res.IsNullOrEmpty())
			{
				return NotFound("Can find this name");
			}
			return Ok(res);
		}

		[HttpPost]
		[Route("AddNew")]
		public async Task<IActionResult> addNew(VacxinCovid vacxincovid)
		{
			var res = _context.VacxinCovid.Where(e => e.VCName.Trim().ToLower() == vacxincovid.VCName.Trim().ToLower()).FirstOrDefault();
			if (res == null)
			{
				_context.VacxinCovid.Add(vacxincovid);
				_context.SaveChanges();
				return Ok("Add successful");
			}
			return Conflict("There is already data in table");
		}

		[HttpPut]
		[Route("Update")]
		public async Task<IActionResult> Update(VacxinCovid vacxincovid)
		{
			var res = _context.VacxinCovid.Where(e => e.ID == vacxincovid.ID).FirstOrDefault();
			var resOfResult = _context.VacxinCovid.Where(e => e.VCName.Trim().ToLower() == vacxincovid.VCName.Trim().ToLower()).FirstOrDefault();
			if (resOfResult == null)
			{
				res.VCName = vacxincovid.VCName;
				_context.VacxinCovid.Update(res);
				_context.SaveChanges();
				return Ok("Update Successfull");
			}
			return Conflict("There is already data in table");
		}
		[HttpPut]
		[Route("SoftDelete")]
		public async Task<IActionResult> Delete(VacxinCovid vacxincovid)
		{
			var res = _context.VacxinCovid.Where(e => e.ID == vacxincovid.ID).FirstOrDefault();
			res.OfStatus = 0;
			_context.VacxinCovid.Update(res);
			_context.SaveChanges();
			return Ok("Delete Successfull");
		}

		[HttpPut]
		[Route("Restore")]
		public async Task<IActionResult> Restore(VacxinCovid vacxincovid)
		{
			var res = _context.VacxinCovid.Where(e => e.ID == vacxincovid.ID).FirstOrDefault();
			res.OfStatus = 1;
			_context.VacxinCovid.Update(res);
			_context.SaveChanges();
			return Ok("Restore Successfull");
		}
	}
}
