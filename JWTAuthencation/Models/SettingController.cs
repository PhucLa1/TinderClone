using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using JWTAuthencation.Data;
using JWTAuthencation.Models;
using Microsoft.IdentityModel.Tokens;
using JWTAuthencation.HelpMethod;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthencation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : Controller
    {
        private readonly JWTAuthencationContext _context;
        public SettingController(JWTAuthencationContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("GetUserSettingById")]
        public async Task<IActionResult> GetUserSettingById(int Id)
        {
            try
            {
                var result = _context.Setting.Where(e => e.Id == Id).FirstOrDefault();
                if (result == null)
                {
                    return NotFound("No data in the table");
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
        public async Task<IActionResult> AddNew(Setting setting)
        {
            try
            {
                var userSetting = _context.Setting.Where(x => x.Id == setting.Id).FirstOrDefault();
                try
                {
                    setting.OfStatus = 1;
                    _context.Setting.Add(setting);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest("Cannot add new file");
                }
                return Ok(setting);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateSetting")]
        public async Task<IActionResult> UpdateSetting(Setting setting)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid data");
                }
                var userSetting = _context.Setting.Where(e => e.Id == setting.Id).FirstOrDefault();
                try
                {
                    userSetting.DistancePreference = setting.DistancePreference;
                    userSetting.LookFor = setting.LookFor;
                    userSetting.AgeMin = setting.AgeMin;
                    userSetting.AgeMax = setting.AgeMax;
                    userSetting.DistanceUnit = setting.DistanceUnit;
                    userSetting.GlobalMatches = setting.GlobalMatches;
                    userSetting.HideAge = setting.HideAge;
                    userSetting.HideDistance = setting.HideDistance;
                    userSetting.Email = setting.Email;
                    userSetting.PhoneNumber = setting.PhoneNumber;
                    _context.Setting.Update(userSetting);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest("Cannot update photo: " + ex.Message);
                }
                return Ok(userSetting);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
    }
}