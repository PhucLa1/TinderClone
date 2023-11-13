using JWTAuthencation.Data;
using JWTAuthencation.Models;
using JWTAuthencation.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System;

namespace JWTAuthencation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MessageController : ControllerBase
	{
		private readonly JWTAuthencationContext _context;
		public MessageController(JWTAuthencationContext context)
		{
			_context = context;
		}
		[HttpGet]
		[Route("GenMessage")]
		public async Task<IActionResult> GenMessage(int number)
		{
			Random rnd = new Random();
			int countUser  = _context.Users.Count();
			var content = _context.SuggestedQuestion.Select(e => e.Ques).ToList();
			var intCombine = content.Count();
			List<Mess> messes = new List<Mess>();
			List<int> valueOfTime = new List<int>(); // Khởi tạo phần tử những giá trị cộng vào randomDate
			valueOfTime.Add(0);  //Thêm vào phần tử đầu tiên 
			for(int i = 1;i<= number; i++)
			{
				int sendUser = rnd.Next(1,countUser);
				int receiveUser;
				do
				{
					receiveUser = rnd.Next(1,countUser);
				} while(receiveUser == sendUser);
				DateTime startDate = new DateTime(2023, 11, 1);
				DateTime endDate = new DateTime(2023, 11, 6);
				int range = (int)(endDate - startDate).TotalSeconds; // Số giây trong khoảng thời gian
				int randomSeconds =  rnd.Next(range);



				// Tạo ngày giờ ngẫu nhiên bằng cách thêm số giây ngẫu nhiên vào ngày bắt đầu
				DateTime randomDate = startDate.AddSeconds(randomSeconds);

				//Tao content ngau nhien
				string contentOfMess = content[rnd.Next(intCombine)];
				Mess mess = new Mess()
				{
					SendUserId = sendUser,
					ReceiveUserId = receiveUser,
					SendTime = randomDate,
					Content = contentOfMess
				};
				messes.Add(mess);
			}
			messes = messes.OrderBy(e => e.SendTime).ToList();
			_context.Mess.AddRange(messes);
			_context.SaveChanges();
			return Ok("Add successfully " + number + " message");
		}

		[HttpGet]
		[Route("GetAll")]
		public async Task<IActionResult> GetAll()
		{
			var res = _context.Mess.OrderBy(e => e.SendTime).ToList();
			return Ok(res);
		}

		[HttpGet]
		[Route("GetCountMess")]
        public async Task<IActionResult> getCountMess()
        {
            var res = _context.GetCountMess.FromSqlRaw("EXEC GetCountMess @Date", new SqlParameter("Date", "2023-11-05")).AsEnumerable().ToList();
            return Ok(res);
        }

		[HttpGet]
		[Route("GetMessByUserID")]
		public async Task<IActionResult> GetMessByUserID(int userId,int toID)
		{
			var res = _context.Mess.
				Where(e => (e.SendUserId == userId && e.ReceiveUserId ==toID)  || (e.ReceiveUserId == userId && e.SendUserId ==toID)).
				OrderBy(e => e.SendTime).
				ToList();
			//Không có chuyện nó res bị rỗng
			return Ok(res);
		}
	}
}
