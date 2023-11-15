using JWTAuthencation.Data;
using JWTAuthencation.Models;
using Microsoft.AspNetCore.SignalR;

namespace JWTAuthencation.Hubs
{
    public class ChatHub : Hub
    {
        private Dictionary<int, string> infoConnect = new Dictionary<int, string>();
		private readonly JWTAuthencationContext _context;
        private readonly IHubContext<ChatHub> _hubContext;
        public ChatHub(JWTAuthencationContext context, IHubContext<ChatHub> hubContext)
		{
			_context = context;
            _hubContext = hubContext;
        }
		public async Task SendMessage(int fromID,int toID, string message)
        {
            Mess mess = new Mess()
            {
                SendUserId = fromID,
                ReceiveUserId = toID,
                Content = message,
                SendTime = DateTime.UtcNow,
            };
            _context.Mess.Add(mess);
            _context.SaveChanges();
            try
            {
                await Clients.Client(infoConnect[toID]).SendAsync("ReceiveMessage", fromID, toID, message);
            }catch (Exception ex)
            {
                throw new Exception();
            }
            
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
            if(infoConnect.ContainsKey(userID))
            {
                infoConnect[userID] = connectionId;
            }
            else
            {
                infoConnect.Add(userID, connectionId);
            }			
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
