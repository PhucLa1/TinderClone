using JWTAuthencation.Data;
using JWTAuthencation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthencation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WorkoutController : ControllerBase
	{
		private readonly JWTAuthencationContext _context;
		public WorkoutController(JWTAuthencationContext context)
		{
			_context = context;
		}
		[HttpGet]
		[Route("GetAll")]
		public async Task<IActionResult> getAllWorkout()
		{

			var res = _context.Workout.ToList();
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
			var res = _context.Workout.Where(e => e.ID == ID && e.OfStatus == 1).FirstOrDefault();
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
			var res = _context.Workout.Where(e => e.WName.Trim().ToLower().Contains(Name.Trim().ToLower())
											&& e.OfStatus == 1).ToList();
			if (res.IsNullOrEmpty())
			{
				return NotFound("Can find this name");
			}
			return Ok(res);
		}

		[HttpPost]
		[Route("AddNew")]
		public async Task<IActionResult> addNew(Workout workout)
		{
			var res = _context.Workout.Where(e => e.WName.Trim().ToLower() == workout.WName.Trim().ToLower()).FirstOrDefault();
			if (res == null)
			{
				_context.Workout.Add(workout);
				_context.SaveChanges();
				return Ok("Add successful");
			}
			return Conflict("There is already data in table");
		}

		[HttpPut]
		[Route("Update")]
		public async Task<IActionResult> Update(Workout workout)
		{
			var res = _context.Workout.Where(e => e.ID == workout.ID).FirstOrDefault();
			var resOfResult = _context.Workout.Where(e => e.WName.Trim().ToLower() == workout.WName.Trim().ToLower()).FirstOrDefault();
			if (resOfResult == null)
			{
				res.WName = workout.WName;
				_context.Workout.Update(res);
				_context.SaveChanges();
				return Ok("Update Successfull");
			}
			return Conflict("There is already data in table");
		}
		[HttpPut]
		[Route("SoftDelete")]
		public async Task<IActionResult> Delete(Workout workout)
		{
			var res = _context.Workout.Where(e => e.ID == workout.ID).FirstOrDefault();
			res.OfStatus = 0;
			_context.Workout.Update(res);
			_context.SaveChanges();
			return Ok("Delete Successfull");
		}

		[HttpPut]
		[Route("Restore")]
		public async Task<IActionResult> Restore(Workout workout)
		{
			var res = _context.Workout.Where(e => e.ID == workout.ID).FirstOrDefault();
			res.OfStatus = 1;
			_context.Workout.Update(res);
			_context.SaveChanges();
			return Ok("Restore Successfull");
		}
	}
}
