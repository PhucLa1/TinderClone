using JWTAuthencation.Data;
using Microsoft.AspNetCore.SignalR;

namespace JWTAuthencation.Hubs
{
    public class ChatHub : Hub
    {
        private Dictionary<int, string> infoConnect = new Dictionary<int, string>();
		private readonly JWTAuthencationContext _context;
		public ChatHub(JWTAuthencationContext context)
		{
			_context = context;
		}
		public async Task SendMessage(int fromID,int toID, string message)
        {           
            await Clients.Client(infoConnect[toID]).SendAsync("ReceiveMessage", fromID,toID,message);
        }
        //Gửi trạng thái trong khi đang gọi điện
        public async Task CameraStateChange(string userId, bool isCameraOn)
        {
            // Gửi tín hiệu trạng thái camera đến tất cả các thành viên trong phòng
            await Clients.All.SendAsync("CameraState", userId, isCameraOn);
        }
		public async Task InitConnect(int userID)
		{
			// Lấy ConnectionId của kết nối hiện tại
			string connectionId = Context.ConnectionId;
			infoConnect.Add(userID, connectionId);

			// Sử dụng connectionId theo nhu cầu của bạn
			// Ví dụ: Gửi thông điệp đến kết nối hiện tại
			await Clients.Client(connectionId).SendAsync("Connect", "Xin chào từ server!, kết nối thành công tới clients "+ connectionId);
		}

		public async Task AudioStateChange(string userId, bool isAudioOn)
        {
            // Gửi tín hiệu trạng thái camera đến tất cả các thành viên trong phòng
            await Clients.All.SendAsync("AudioState", userId, isAudioOn);
        }

        //Gửi trạng thái trước khi gọi điện
        public async Task CallWait(int toID)
        {
            await Clients.Client(infoConnect[toID]).SendAsync("CallWaitUser", toID);
        }
        public async Task CallAnswer(string userId,int from,int to,bool Ans)
        {
            // Gửi tín hiệu trạng thái camera đến tất cả các thành viên trong phòng
            await Clients.All.SendAsync("CallAnswerUser", userId,from,to,Ans);
        }
		public async Task EndCall(string userId, int from,int to)
		{
			// Gửi tín hiệu trạng thái camera đến tất cả các thành viên trong phòng
			await Clients.All.SendAsync("CallAnswerUser", userId, from,to);
		}
	}
}
