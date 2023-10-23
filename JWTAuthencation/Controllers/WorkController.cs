using JWTAuthencation.Data;
using JWTAuthencation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthencation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkController : ControllerBase
    {
        private readonly JWTAuthencationContext _context;
        public WorkController(JWTAuthencationContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = _context.Work.ToList();
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
            var result = _context.Work.Where(e => e.Id == Id).FirstOrDefault();
            if (result == null)
            {
                return Ok("No data in the table");
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("AddNew")]
        public async Task<IActionResult> AddNew(Work Work)
        {
            var result = _context.Work.Where(e => e.Wname == Work.Wname).FirstOrDefault();
            if (result == null)
            {
                _context.Work.Add(Work);
                _context.SaveChanges();
                return Ok(Work);
            }
            return Ok("There is already exist name in table");
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(Work Work)
        {
            var result = _context.Work.Where(e => e.Wname == Work.Wname).ToList();
            if (result.Count == 1)
            {
                return Ok("There is already exist name in table");
            }
            else
            {
                var resOfUpdate = _context.Work.Where(e => e.Id == Work.Id).FirstOrDefault();

                resOfUpdate.Wname = Work.Wname;
                resOfUpdate.Descriptions = Work.Descriptions;
                //resOfUpdate.OfStatus = Work.OfStatus;

                _context.Work.Update(resOfUpdate);
                _context.SaveChanges();
                return Ok(resOfUpdate);
            }
        }


        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Work Work)
        {
            var result = _context.Work.Where(e => e.Id == Work.Id).FirstOrDefault();
            _context.Work.Remove(result);
            _context.SaveChanges();
            return Ok(result);

        }
    }
}
