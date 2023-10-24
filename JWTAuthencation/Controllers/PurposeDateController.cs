﻿using JWTAuthencation.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthencation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurposeDateController : ControllerBase
    {
        private readonly JWTAuthencationContext _context;
        public PurposeDateController(JWTAuthencationContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> getAll()
        {
            var res = _context.PurposeDate.ToList();
            if (res.IsNullOrEmpty())
            {
                return Ok("No data in the table");
            }
            return Ok(res);
        }
    }
}
