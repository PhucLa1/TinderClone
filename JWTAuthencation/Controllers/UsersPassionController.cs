using JWTAuthencation.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthencation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersPassionController : ControllerBase
	{
		private readonly JWTAuthencationContext _context;
		public UsersPassionController(JWTAuthencationContext context)
		{
			_context = context;
		}

		[HttpPut]
		[Route("UpdatePassions")]
		public async Task<IActionResult> UpdatePassions(int id, List<int> passionID)
		{
			var uPs = _context.UsersPassion.Where(e => e.UserId == id).ToList();
			int countP = passionID.Count;
			bool hasDuplicates = passionID.Count != passionID.Distinct().Count();
			if (hasDuplicates == true)
			{
				return Conflict("Passions has duplicate id");
			}
			//Update range luôn
			if (countP == 5)
			{
				for (int i = 0; i < 5; i++)
				{
					uPs[i].PassionId = passionID[i];
				}
				_context.UpdateRange(uPs);
			}
			else if (countP < 5)
			{

				for (int i = 0; i < 5; i++)
				{
					if (i < countP)
					{
						uPs[i].PassionId = passionID[i];
					}
					else
					{
						uPs[i].PassionId = null;
					}
				}
				_context.UpdateRange(uPs);
			}
			_context.SaveChanges();
			return Ok("Update Successfully");
		}
	}
}
