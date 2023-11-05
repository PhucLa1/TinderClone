using JWTAuthencation.Data;
using JWTAuthencation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthencation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersLanguagesController : ControllerBase
	{
		//Phần này chỉ có update thôi, chứ không có xóa hay thêm vì lúc thêm đã thêm vào lúc signup rồi
		private readonly JWTAuthencationContext _context;
		public UsersLanguagesController(JWTAuthencationContext context)
		{
			_context = context;
		}

		[HttpPut]
		[Route("UpdateLanguages")]
		public async Task<IActionResult> UpdateLanguages(int id,List<int> languageID )
		{
			var uLs = _context.UsersLanguages.Where(e => e.UserId == id).ToList();
			int countL = languageID.Count;
			bool hasDuplicates = languageID.Count != languageID.Distinct().Count();
			if (hasDuplicates == true)
			{
				return Conflict("Languages has duplicate id");
			}
			//Update range luôn
			if (countL == 5)
			{
				for (int i = 0; i < 5; i++)
				{
					uLs[i].LanguageId = languageID[i];
				}		
				_context.UpdateRange(uLs);
			}
			else if(countL < 5)
			{
				
				for (int i = 0; i < 5; i++)
				{
					if (i < countL)
					{
						uLs[i].LanguageId = languageID[i];
					}
					else
					{
						uLs[i].LanguageId = null;
					}
				}
				_context.UpdateRange(uLs);
			}
			_context.SaveChanges();
			return Ok("Update Successfully");
		}
	}
}
