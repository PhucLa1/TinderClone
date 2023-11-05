using JWTAuthencation.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthencation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SuggestedQuestionController : ControllerBase
	{
		private readonly JWTAuthencationContext _context;
		public SuggestedQuestionController(JWTAuthencationContext context)
		{
			_context = context;
		}

		//Gen ra 1 câu hỏi ngẫu nhiên
		[HttpGet]
		[Route("GetRandomQues")]
		public async Task<IActionResult> GetRandom()
		{
			try
			{
				Random random = new Random();
				var res = _context.SuggestedQuestion.ToList();
				int countQues = res.Count();
				int randomQues = random.Next(1, countQues);

				var resOfQues = res.Where(e => e.Id == randomQues).FirstOrDefault();
				return Ok(resOfQues);

			}
			catch(Exception ex)
			{
				return StatusCode(500, ex.Message);
			}

		}
		
		
	}
}
