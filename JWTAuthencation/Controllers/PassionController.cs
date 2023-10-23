using JWTAuthencation.Data;
using JWTAuthencation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthencation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassionController : ControllerBase
    {
        private readonly JWTAuthencationContext _context;
        public PassionController(JWTAuthencationContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = _context.Passion.ToList();
            if (result.IsNullOrEmpty())
            {
                return Ok("no data in the table");
            }
             return Ok(result);          
        }

        [HttpGet]
        [Route("GetByID/{Id}")]
        public async Task<IActionResult> GetByID(int Id)
        {
            var result = _context.Passion.Where(e => e.Id == Id).FirstOrDefault();
            if (result == null)
            {
                return Ok("No data in the table");
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("AddNew")]
        public async Task<IActionResult> AddNew(Passion passion)
        {
            var result = _context.Passion.Where(e => e.Pname == passion.Pname).FirstOrDefault();
            if (result == null)
            {
                _context.Passion.Add(passion);
                _context.SaveChanges();
                return Ok(passion);               
            }
            return Ok("There is already exist name in table");
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(Passion passion)
        {
            var result = _context.Passion.Where(e => e.Pname == passion.Pname).ToList();
            if (result.Count == 1)
            {
                return Ok("There is already exist name in table");
            }
            else
            {
                var resOfUpdate = _context.Passion.FirstOrDefault(e => e.Id == passion.Id);

                resOfUpdate.Pname = passion.Pname;
                resOfUpdate.Descriptions = passion.Descriptions;
                //resOfUpdate.OfStatus = passion.OfStatus;

                _context.Passion.Update(resOfUpdate);
                _context.SaveChanges();
                return Ok(resOfUpdate);
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Passion passion)
        {
            var result = _context.Passion.Where(e => e.Id == passion.Id).FirstOrDefault();
            _context.Passion.Update(result);
            _context.SaveChanges();
            return Ok(result);

        }
    }
}
