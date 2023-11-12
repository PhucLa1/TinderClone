using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JWTAuthencation.Data;
using JWTAuthencation.Models;
using Microsoft.IdentityModel.Tokens;
using JWTAuthencation.HelpMethod;
using System.Net.NetworkInformation;

namespace JWTAuthencation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PhotoController : ControllerBase
	{
		private readonly JWTAuthencationContext _context;

		public PhotoController(JWTAuthencationContext context)
		{
			_context = context;
		}
		[HttpGet]
		[Route("GetAll")]
		public async Task<IActionResult> GetAll(int UserId)
		{
			try
			{
				var result = _context.Photo.Where(e => e.UserId == UserId).ToList();
				if (result.IsNullOrEmpty())
				{
					return NotFound("No data in the table");
				}
				else
				{
					string protocol = Request.Scheme;
					string host = Request.Host.ToString();
					string url = protocol + "://" + host;
					result.ForEach(item =>
					{
						item.ImagePath = url + "/Uploads/" + item.ImagePath;
					});
				}
				return Ok(result);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error: " + ex.Message);
			}
		}
		[HttpGet]
		[Route("GetByID")]
		public async Task<IActionResult> GetByID(int ImageId)
		{
			try
			{
				var result = _context.Photo.Where(e => e.Id == ImageId).FirstOrDefault();
				if (result == null)
				{
					return NotFound("No data in the table");
				}
				else
				{
					string protocol = Request.Scheme;
					string host = Request.Host.ToString();
					string url = protocol + "://" + host;
					result.ImagePath = url + "/Uploads/" + result.ImagePath;
				}
				return Ok(result);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error: " + ex.Message);
			}
		}

		[HttpPost]
		[Route("AddNew")]
		public async Task<IActionResult> AddNew([FromForm] Photo photo)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest("Invalid data");
				}
				var result = _context.Photo.Where(e => e.UserId == photo.UserId).Count();
				if (result >= 9)
                {
					return BadRequest("cannot update more image");
				}

				else
				{
					string uploadedFileName = await HandleImage.Upload(photo.Image);
					if (uploadedFileName == null)
					{
						return StatusCode(500, "Internal Server Error: Failed to upload image");
					}
					var image = new Photo { OfStatus = 1, UserId = photo.UserId };
					image.ImagePath = uploadedFileName;
					_context.Photo.Add(image);
					_context.SaveChanges();

					return Ok(image);
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error: " + ex.Message);
			}
		}
		[HttpDelete]
		[Route("Delete")]
		public async Task<IActionResult> Delete(Photo photo)
		{
			try
			{
				var result = _context.Photo.Where(e => e.Id == photo.Id).FirstOrDefault();
				var countImg = _context.Photo.Where(e => e.UserId == photo.UserId).ToList();
				if (countImg.Count == 1)
				{
					return Forbid("Cannot delete last photo");
				}
				if (result != null)
				{
					string path = result.ImagePath;
					string fullpath = "wwwroot/Uploads/" + path;
					try
					{
						if (System.IO.File.Exists(fullpath))
						{
							System.IO.File.Delete(fullpath);
						}
					}
					catch (Exception ex)
					{
						return BadRequest("Cannot delete file: " + ex.Message);
					}
					try
					{
						_context.Photo.Remove(result);
						_context.SaveChanges();
					}
					catch (Exception ex)
					{
						return Forbid("Cannot delete photo from database: " + ex.Message);
					}
					return Ok(result);
				}
				else
				{
					return NotFound("Photo doesn't exsist");
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error: " + ex.Message);
			}
		}
		[HttpPut]
		[Route("Update")]
		public async Task<IActionResult> Update([FromForm] Photo photo)
		{
			try
			{
				var result = _context.Photo.Where(e => e.Id == photo.Id).FirstOrDefault();
				if (result != null)
				{
					string path = result.ImagePath;
					string fullpath = "wwwroot/Uploads/" + path;
					string uploadedFileName = await HandleImage.Upload(photo.Image);

					if (uploadedFileName == null)
					{
						return StatusCode(500, "Internal Server Error: Failed to upload image");
					}

					photo.ImagePath = uploadedFileName;
					try
					{
						result.UserId = photo.UserId;
						result.ImagePath = photo.ImagePath;
						_context.Photo.Update(result);
						_context.SaveChanges();
					}
					catch (Exception ex)
					{
						return BadRequest("Cannot update photo: " + ex.Message);
					}

					try
					{
						if (System.IO.File.Exists(fullpath))
						{
							System.IO.File.Delete(fullpath);
						}
					}
					catch (Exception ex)
					{
						return BadRequest("Cannot delete photo from database: " + ex.Message);
					}
					_context.SaveChanges();
					return Ok("Update Successfull");
				}
				else
				{
					return NotFound("Photo does not exist");
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error: " + ex.Message);
			}
		}
	}
}