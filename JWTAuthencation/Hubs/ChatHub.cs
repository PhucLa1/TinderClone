using JWTAuthencation.Data;
using JWTAuthencation.Models;
using JWTAuthencation.Models.ViewModel;
using Microsoft.AspNetCore.SignalR;

namespace JWTAuthencation.Hubs
{
    public class ChatHub : Hub
    {
        public static Dictionary<int, string> infoConnect = new Dictionary<int, string>();
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
                SendTime = DateTime.UtcNow.AddHours(7),
            };
            _context.Mess.Add(mess);
            _context.SaveChanges();
            try
            {
                await Clients.All.SendAsync("ReceiveMessage", fromID, toID, message);
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
        // Người dùng bấm gọi và hiện ra popup gọi
        public async Task CallWait(int fromID,int toID)
        {
            //Lấy thông tin của thằng đang gọi đến cho thằng toID( tức là thằng FromID)
            UserCall uC = new UserCall()
            {
                ID = fromID,
                FullName = _context.Users.Where(e => e.Id == fromID).Select(e => e.FullName).FirstOrDefault(),
                ImagePath = _context.Photo.Where(e => e.UserId == fromID).Select(e => "https://localhost:7251/Uploads/"+e.ImagePath).FirstOrDefault()
            };
            await Clients.Client(infoConnect[toID]).SendAsync("CallWaitUser", uC);
            //Trả về thằng cần thông tin đang gọi
        }
        public async Task MislabeledCall(int fromID, int toID)
        {
            //fromID ở đây là thằng gọi, toID ở đây là thằng được gọi
            Call call = new Call()
            {
                CallerId = fromID,
                ReceiverId = toID,
                StartTime = DateTime.UtcNow.AddHours(7),
                EndTime = DateTime.UtcNow.AddHours(7),
                Duration = 0,
                CallStatusId = 2
            };
            _context.Calls.Add(call);
            _context.SaveChanges();
            //Cuộc gọi nhầm
            Mess mess = new Mess()
            {
                SendUserId = fromID,
                ReceiveUserId = toID,
                SendTime = DateTime.UtcNow.AddHours(7),
                Content = "<span className='text-red-900'>Mislabeled Call</span>"
            };
            _context.Mess.Add(mess);
            _context.SaveChanges();
            await Clients.Client(infoConnect[toID]).SendAsync("MislabeledCallUser");

        }
        public async Task InitCall(int agreedID,int agreeID)
        {
            //Đây chính là thằng mà mình đăng nhập vào
            //Code này chỉ để khởi tạo hình ảnh và âm thanh cho bên 2 thằng gọi trước
            await Clients.Client(infoConnect[agreeID]).SendAsync("InitCallUser",agreeID,agreedID);
        }
        public async Task RejectCall(int fromID,int toID)
        {
            //fromID ở đây là thằng từ chối, toID là thằng bị từ chối
            //Đồng nghĩa với việc là toID là thằng gọi tới , fromID là thằng nhận cuộc gọi
            Call call = new Call()
            {
                CallerId = toID,
                ReceiverId = fromID,
                StartTime = DateTime.UtcNow.AddHours(7),
                EndTime = DateTime.UtcNow.AddHours(7),
                Duration = 0,
                CallStatusId = 1
            };
            _context.Calls.Add(call);
            
            // 1 : Cuộc gọi bị từ chối
            // 2: Cuộc gọi nhầm
            //3 : Cuộc gọi thành công
            //Gửi lên client
            //toID là thằng gọi tới, vậy nên sẽ gửi dữ liệu của thằng fromID về cho toID
            UserCall uReject = new UserCall()
            {
                ID = fromID,
                FullName = _context.Users.Where(e => e.Id == fromID).Select(e => e.FullName).FirstOrDefault(),
                ImagePath = _context.Photo.Where(e => e.UserId == fromID).Select(e => "https://localhost:7251/Uploads/" + e.ImagePath).FirstOrDefault()
            };

            //Lưu vào trong db text về mảng tin nhắn
            Mess mess = new Mess()
            {
                SendUserId = fromID,
                ReceiveUserId = toID,
                SendTime = DateTime.UtcNow.AddHours(7),
                Content = "<span className='text-red-900'>Call has to be reject</span>"
            };
            _context.Mess.Add(mess);
            _context.SaveChanges();
            await Clients.Client(infoConnect[toID]).SendAsync("RejectCallUser", uReject);
        }
        public async Task AgreeCall(string peerID,int fromID,int toID)
        {
            // fromID là thằng gọi đến, toID là thằng nhận cuộc gọi
            await Clients.All.SendAsync("AgreeCallUser", peerID, fromID, toID);
        }
		public async Task EndCall(string peerID)
		{
			await Clients.All.SendAsync("EndCallUser", peerID);
		}
    }
}
