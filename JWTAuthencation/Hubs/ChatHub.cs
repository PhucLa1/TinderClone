using JWTAuthencation.Models;
using Microsoft.AspNetCore.SignalR;

namespace JWTAuthencation.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task CameraStateChange(string userId, bool isCameraOn)
        {
            // Gửi tín hiệu trạng thái camera đến tất cả các thành viên trong phòng
            await Clients.All.SendAsync("CameraState", userId, isCameraOn);
        }
    }
}
