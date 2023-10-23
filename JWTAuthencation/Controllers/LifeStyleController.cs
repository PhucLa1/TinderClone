using JWTAuthencation.Data;
using JWTAuthencation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthencation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LifeStyleController : ControllerBase
    {
        private readonly JWTAuthencationContext _context;
        public LifeStyleController(JWTAuthencationContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = _context.LifeStyle.ToList();
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
            var result = _context.LifeStyle.Where(e => e.Id == Id).FirstOrDefault();
            if (result == null)
            {
                return Ok("No data in the table");
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("AddNew")]
        public async Task<IActionResult> AddNew(LifeStyle lifeStyle)
        {
            var result = _context.LifeStyle.Where(e => e.Lsname == lifeStyle.Lsname).FirstOrDefault();
            if (result == null)
            {
                _context.LifeStyle.Add(lifeStyle);
                _context.SaveChanges();
                return Ok(lifeStyle);
            }
            return Ok("There is already exist name in table");
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(LifeStyle lifeStyle)
        {
            var result = _context.LifeStyle.Where(e => e.Lsname == lifeStyle.Lsname).ToList();
            if (result.Count == 1)
            {
                return Ok("There is already exist name in table");
            }
            else
            {
                var resOfUpdate = _context.LifeStyle.FirstOrDefault(e => e.Id == lifeStyle.Id);
                //Gán giá trị
                resOfUpdate.Lsname =lifeStyle.Lsname;
                //resOfUpdate.OfStatus = lifeStyle.OfStatus;
                resOfUpdate.Descriptions = lifeStyle.Descriptions;

                _context.LifeStyle.Update(resOfUpdate);
                _context.SaveChanges();
                return Ok(resOfUpdate);
            }
        }


        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(LifeStyle lifeStyle)
        {
            var result = _context.LifeStyle.Where(e => e.Id == lifeStyle.Id).FirstOrDefault();
            _context.LifeStyle.Remove(result);
            _context.SaveChanges();
            return Ok(result);

        }
    }
}
