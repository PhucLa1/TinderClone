using JWTAuthencation.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JWTAuthencation.Models;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthencation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoveLanguageController : ControllerBase
	{
		private readonly JWTAuthencationContext _context;
		public LoveLanguageController(JWTAuthencationContext context)
		{
			_context = context;
		}
		[HttpGet]
		[Route("GetAll")]
		public async Task<IActionResult> getAllLLanguage()
		{
			var res = _context.LoveLanguage.ToList();
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
			var res = _context.LoveLanguage.Where(e => e.ID == ID && e.OfStatus == 1).FirstOrDefault();
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
			var res = _context.LoveLanguage.Where(e => e.LLName.Trim().ToLower().Contains(Name.Trim().ToLower())
											&& e.OfStatus == 1).ToList();
			if (res.IsNullOrEmpty())
			{
				return NotFound("Can find this name");
			}
			return Ok(res);
		}

		[HttpPost]
		[Route("AddNew")]
		public async Task<IActionResult> addNew(LoveLanguage lovelanguage)
		{
			var res = _context.LoveLanguage.Where(e => e.LLName.Trim().ToLower() == lovelanguage.LLName.Trim().ToLower()).FirstOrDefault();
			if (res == null)
			{
				_context.LoveLanguage.Add(lovelanguage);
				_context.SaveChanges();
				return Ok("Add successful");
			}
			return Conflict("There is already data in table");
		}

		[HttpPut]
		[Route("Update")]
		public async Task<IActionResult> Update(LoveLanguage lovelanguage)
		{
			var res = _context.LoveLanguage.Where(e => e.ID == lovelanguage.ID).FirstOrDefault();
			var resOfResult = _context.LoveLanguage.Where(e => e.LLName.Trim().ToLower() == lovelanguage.LLName.Trim().ToLower()).FirstOrDefault();
			if (resOfResult == null)
			{
				res.LLName = lovelanguage.LLName;
				_context.LoveLanguage.Update(res);
				_context.SaveChanges();
				return Ok("Update Successfull");
			}
			return Conflict("There is already data in table");
		}
		[HttpPut]
		[Route("SoftDelete")]
		public async Task<IActionResult> Delete(LoveLanguage lovelanguage)
		{
			var res = _context.LoveLanguage.Where(e => e.ID == lovelanguage.ID).FirstOrDefault();
			res.OfStatus = 0;
			_context.LoveLanguage.Update(res);
			_context.SaveChanges();
			return Ok("Delete Successfull");
		}

		[HttpPut]
		[Route("Restore")]
		public async Task<IActionResult> Restore(LoveLanguage lovelanguage)
		{
			var res = _context.LoveLanguage.Where(e => e.ID == lovelanguage.ID).FirstOrDefault();
			res.OfStatus = 1;
			_context.LoveLanguage.Update(res);
			_context.SaveChanges();
			return Ok("Restore Successfull");
		}
	}
}