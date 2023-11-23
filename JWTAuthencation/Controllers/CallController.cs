//using JWTAuthencation.Data;
//using JWTAuthencation.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace JWTAuthencation.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CallController : ControllerBase
//    {
//        private readonly JWTAuthencationContext _context;
//        public CallController(JWTAuthencationContext context) { _context = context; }
//        [HttpPost]
//        [Route("Add")]
//        public async Task<IActionResult> AddNew(Call call)
//        {
//            //Call call = new Call()
//            //{
//            //    CallerId = 1,
//            //    ReceiverId = 1,
//            //    StartTime = DateTime.UtcNow,
//            //    EndTime = DateTime.UtcNow,
//            //    Duration = 0,
//            //    CallStatusId = 1
//            //};
//            _context.Calls.Add(call);
//            Mess mess = new Mess()
//            {
//                SendUserId = 
//            }
//            _context.SaveChanges();
//            return Ok();
//        }
//    }
//}
