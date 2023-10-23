using JWTAuthencation.Data;
using JWTAuthencation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthencation.wwwroot
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagesController : ControllerBase
    {
        private readonly JWTAuthencationContext _context;
        public LanguagesController(JWTAuthencationContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = _context.Languages.ToList();
            if (result.IsNullOrEmpty())
            {
                return Ok("No data in the table");
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("GetByID/{Id}")]
        public async Task<IActionResult> GetByID(int Id)
        {
            var result = _context.Languages.Where(e => e.Id == Id).FirstOrDefault();
            if (result == null)
            {
                return Ok("No data in the table");
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("AddNew")]
        public async Task<IActionResult> AddNew(Languages Languages)
        {
            var result = _context.Languages.Where(e => e.Lname == Languages.Lname).FirstOrDefault();
            if (result == null)
            {
                _context.Languages.Add(Languages);
                _context.SaveChanges();
                return Ok(Languages);
            }
            return Ok("There is already exist name in table");
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(Languages Languages)
        {
            var result = _context.Languages.Where(e => e.Lname == Languages.Lname).ToList();
            if (result.Count == 1)
            {
                return Ok("There is already exist name in table");
            }
            else
            {
                var resOfUpdate = _context.Languages.Where(e => e.Id == Languages.Id).FirstOrDefault();

                resOfUpdate.Lname = Languages.Lname;
                resOfUpdate.Descriptions= Languages.Descriptions;
                //resOfUpdate.OfStatus = Languages.OfStatus;

                _context.Languages.Update(resOfUpdate);
                _context.SaveChanges();
                return Ok(resOfUpdate);
            }
        }


        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Languages Languages)
        {
            var result = _context.Languages.Where(e => e.Id == Languages.Id).FirstOrDefault();
            _context.Languages.Remove(result);
            _context.SaveChanges();
            return Ok(result);

        }
    }
}
