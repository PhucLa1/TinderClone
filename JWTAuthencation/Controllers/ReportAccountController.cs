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
    public class ReportAccountController : ControllerBase
    {
        private readonly JWTAuthencationContext _context;
        public ReportAccountController(JWTAuthencationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var res = _context.ReportAccount.ToList();
                if (res.IsNullOrEmpty())
                {
                    return NotFound("No data in the table");
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
        [HttpPost]
        [HttpPost]
        [Route("ReportUser")]
        public async Task<IActionResult> ReportUser([FromBody] ReportAccount reportAccount)
        {
            try
            {
                if (reportAccount == null)
                {
                    return BadRequest("Report account is required.");
                }

                if (reportAccount.ReportedUserID == null)
                {
                    return BadRequest("Reported user ID is required.");
                }

                if (reportAccount.Reason == null)
                {
                    return BadRequest("Report reason is required.");
                }

                var reportedUser = _context.Users.Where(e => e.Id == reportAccount.ReportedUserID).FirstOrDefault();
                if (reportedUser == null)
                {
                    return NotFound($"User with ID {reportAccount.ReportedUserID} does not exist.");
                }

                var reportingUser = _context.Users.Where(e => e.Id == reportAccount.IdReportFrom).FirstOrDefault();
                if (reportingUser == null)
                {
                    return NotFound($"User with ID {reportAccount.IdReportFrom} does not exist.");
                }

                _context.ReportAccount.Add(reportAccount);
                await _context.SaveChangesAsync();
                return Ok("Report added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
        [HttpPut]
        [Route("UpdateReport")]
        public async Task<IActionResult> UpdateReport(ReportAccount reportAccount)
        {
            try
            {
                var result = _context.ReportAccount.Where(e => e.ID == reportAccount.ID).FirstOrDefault();
                if (reportAccount == null)
                {
                    return BadRequest("Report account is required.");
                }

                if (reportAccount.ID == null)
                {
                    return BadRequest("Report ID is required.");
                }
                if (result == null)
                {
                    return NotFound($"Report account with ID {reportAccount.ID} does not exist.");
                }
                else
                {
                    result.OfStatus = reportAccount.OfStatus;
                    _context.ReportAccount.Update(result);
                    _context.SaveChanges();
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
        [HttpPut]
        [Route("UpdateReport/{ReportID}")]
        public async Task<IActionResult> UpdateReport(int ReportID)
        {
            try
            {
                var result = _context.ReportAccount.Where(e => e.ID == ReportID).FirstOrDefault();

                if (result == null)
                {
                    return BadRequest("Report account does not exist.");
                }

                if (result.OfStatus != 0 && result.OfStatus != null)
                {
                    return BadRequest("Report account has already been confirmed.");
                }

                result.OfStatus = 1;
                var violationSuspension = new ViolationSuspension
                {
                    UserID = result.IdReportFrom,
                    ReportID = result.ReportedUserID,
                    Suspended_at = DateTime.Now,
                    OfStatus = 1
                };

                _context.ViolationSuspension.Add(violationSuspension);
                var user = _context.Users.Find(result.ReportedUserID);

                user.IsBlocked = true;
                _context.ReportAccount.Update(result);
                _context.SaveChanges();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(ReportAccount reportAccount)
        {
            // Kiểm tra xem tài khoản cần báo cáo có tồn tại hay không.
            try
            {
                var result = _context.ReportAccount.Where(e => e.ID == reportAccount.ID).FirstOrDefault();
                if (result == null)
                {
                    return BadRequest("Report Account does not exist");
                }
                try
                {
                    _context.ReportAccount.Update(result);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest("Cannot update Report Account: " + ex.Message);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
    }
}