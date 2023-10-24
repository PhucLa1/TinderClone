using JWTAuthencation.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthencation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunicationController : ControllerBase
    {
        private readonly JWTAuthencationContext _context;
        public CommunicationController(JWTAuthencationContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> getAllCommunication()
        {
            var res = _context.Communication.ToList();
            if (res.IsNullOrEmpty())
            {
                return Ok("No data in the table");
            }
            return Ok(res);
        }
    }
}
