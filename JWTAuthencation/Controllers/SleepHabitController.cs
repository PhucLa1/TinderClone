﻿using JWTAuthencation.Data;
using JWTAuthencation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthencation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SleepHabitController : ControllerBase
	{
		private readonly JWTAuthencationContext _context;
		public SleepHabitController(JWTAuthencationContext context)
		{
			_context = context;
		}
		[HttpGet]
		[Route("GetAll")]
		public async Task<IActionResult> getAllSHabit()
		{
			var res = _context.SleepHabit.ToList();
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
			var res = _context.SleepHabit.Where(e => e.ID == ID && e.OfStatus == 1).FirstOrDefault();
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
			var res = _context.SleepHabit.Where(e => e.SHName.Trim().ToLower().Contains(Name.Trim().ToLower())
											&& e.OfStatus == 1).ToList();
			if (res.IsNullOrEmpty())
			{
				return NotFound("Can find this name");
			}
			return Ok(res);
		}

		[HttpPost]
		[Route("AddNew")]
		public async Task<IActionResult> addNew(SleepHabit sleephabit)
		{
			var res = _context.SleepHabit.Where(e => e.SHName.Trim().ToLower() == sleephabit.SHName.Trim().ToLower()).FirstOrDefault();
			if (res == null)
			{
				_context.SleepHabit.Add(sleephabit);
				_context.SaveChanges();
				return Ok("Add successful");
			}
			return Conflict("There is already data in table");
		}

		[HttpPut]
		[Route("Update")]
		public async Task<IActionResult> Update(SleepHabit sleephabit)
		{
			var res = _context.SleepHabit.Where(e => e.ID == sleephabit.ID).FirstOrDefault();
			var resOfResult = _context.SleepHabit.Where(e => e.SHName.Trim().ToLower() == sleephabit.SHName.Trim().ToLower()).FirstOrDefault();
			if (resOfResult == null)
			{
				res.SHName = sleephabit.SHName;
				_context.SleepHabit.Update(res);
				_context.SaveChanges();
				return Ok("Update Successfull");
			}
			return Conflict("There is already data in table");
		}
		[HttpPut]
		[Route("SoftDelete")]
		public async Task<IActionResult> Delete(SleepHabit sleephabit)
		{
			var res = _context.SleepHabit.Where(e => e.ID == sleephabit.ID).FirstOrDefault();
			res.OfStatus = 0;
			_context.SleepHabit.Update(res);
			_context.SaveChanges();
			return Ok("Delete Successfull");
		}

		[HttpPut]
		[Route("Restore")]
		public async Task<IActionResult> Restore(SleepHabit sleephabit)
		{
			var res = _context.SleepHabit.Where(e => e.ID == sleephabit.ID).FirstOrDefault();
			res.OfStatus = 1;
			_context.SleepHabit.Update(res);
			_context.SaveChanges();
			return Ok("Restore Successfull");
		}
	}
}
