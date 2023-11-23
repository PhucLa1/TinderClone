using JWTAuthencation.Data;
using JWTAuthencation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthencation.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ZodiacController : ControllerBase
    {
        private readonly JWTAuthencationContext _context;
        public ZodiacController(JWTAuthencationContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> getAll()
        {
            var res = _context.Zodiac.Where(e =>e.OfStatus == 1).ToList();
            if (res.IsNullOrEmpty())
            {
                return Ok("No data in the table");
            }
            return Ok(res);
        }

        [HttpGet]
        [Route("GetByID")]
        public async Task<IActionResult> getByID(int ID)
        {
            var res = _context.Zodiac.Where(e => e.ID == ID && e.OfStatus == 1).FirstOrDefault();
            if (res == null)
            {
                return Ok("Can find by ID");
            }
            return Ok(res);
        }

        [HttpGet]
        [Route("GetByName")]
        public async Task<IActionResult> getByName(string Name)
        {
            var res = _context.Zodiac.Where(e => e.ZName.Trim().ToLower().Contains(Name.Trim().ToLower())
                                            && e.OfStatus ==1).ToList();
            if (res.IsNullOrEmpty())
            {
                return Ok("Can find this name");
            }
            return Ok(res);
        }

        [HttpPost]
        [Route("AddNew")]
        public async Task<IActionResult> addNew(Zodiac zodiac)
        {
            var res = _context.Zodiac.Where(e =>e.ZName.Trim().ToLower() ==zodiac.ZName.Trim().ToLower()).FirstOrDefault();
            if(res == null)
            {
                _context.Zodiac.Add(zodiac);
                _context.SaveChanges();
                return Ok("Add successful");
            }
            return Ok("There is already data in table");          
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(Zodiac zodiac)
        {
            var res = _context.Zodiac.Where(e =>e.ID == zodiac.ID).FirstOrDefault();
            var resOfResult = _context.Zodiac.Where(e => e.ZName.Trim().ToLower() == zodiac.ZName.Trim().ToLower()).FirstOrDefault();
            if( resOfResult == null)
            {
                res.ZName = zodiac.ZName;
                _context.Zodiac.Update(res);
                _context.SaveChanges();
                return Ok("Update Successfull");
            }
            return Ok("There is already data in table");
        }
        [HttpPut]
        [Route("SoftDelete")]
        public async Task<IActionResult> Delete(Zodiac zodiac)
        {
            var res = _context.Zodiac.Where(e => e.ID == zodiac.ID).FirstOrDefault();
            res.OfStatus = 0;
            _context.Zodiac.Update(res);
            _context.SaveChanges();
            return Ok("Delete Successfull");
        }

        [HttpPut]
        [Route("Restore")]
        public async Task<IActionResult> Restore(Zodiac zodiac)
        {
            var res = _context.Zodiac.Where(e => e.ID == zodiac.ID).FirstOrDefault();
            res.OfStatus = 1;
            _context.Zodiac.Update(res);
            _context.SaveChanges();
            return Ok("Restore Successfull");
        }

    }
}
