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
    public class AdminController : ControllerBase
    {
        private readonly JWTAuthencationContext _context;
        public AdminController(JWTAuthencationContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var res = _context.Admin.Where(e => e.OfStatus == 1).ToList();
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
            var res = _context.Admin.Where(e => e.ID == ID && e.OfStatus == 1).FirstOrDefault();
            if (res == null)
            {
                return Ok("Can find by ID");
            }
            return Ok(res);
        }



        [HttpPost]
        [Route("AddNew")]
        public async Task<IActionResult> addNew(Admin admin)
        {
            var res = _context.Admin.Where(e => e.UserName.Trim().ToLower() == admin.UserName.Trim().ToLower()).FirstOrDefault();
            if (res == null)
            {
                _context.Admin.Add(admin);
                _context.SaveChanges();
                return Ok("Add successful");
            }
            return Ok("There is already data in table");
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(Admin admin)
        {
            var res = _context.Admin.Where(e => e.ID == admin.ID).FirstOrDefault();
            var resOfResult = _context.Admin.Where(e => e.UserName.Trim().ToLower() == admin.UserName.Trim().ToLower()).FirstOrDefault();
            if (resOfResult == null)
            {
                res.UserName = admin.UserName;
                res.Pass = admin.Pass;
                _context.Admin.Update(res);
                _context.SaveChanges();
                return Ok("Update Successfull");
            }
            return Ok("There is already data in table");
        }
        [HttpPut]
        [Route("SoftDelete")]
        public async Task<IActionResult> Delete(Admin admin)
        {
            var res = _context.Admin.Where(e => e.ID == admin.ID).FirstOrDefault();
            res.OfStatus = 0;
            _context.Admin.Update(res);
            _context.SaveChanges();
            return Ok("Delete Successfull");
        }

        [HttpPut]
        [Route("Restore")]
        public async Task<IActionResult> Restore(Admin admin)
        {
            var res = _context.Admin.Where(e => e.ID == admin.ID).FirstOrDefault();
            res.OfStatus = 1;
            _context.Admin.Update(res);
            _context.SaveChanges();
            return Ok("Restore Successfull");
        }

        [HttpGet]
        [Route("CheckPass")]
        public async Task<IActionResult> CheckPass(int adminId,string pass)
        {
            var res = _context.Admin.Where(e => e.Pass == pass && e.ID == adminId).FirstOrDefault();
            if(res == null)
            {
                return NotFound("Password donn't correct");
            }
            return Ok("Password correct");
        }

    }
}
