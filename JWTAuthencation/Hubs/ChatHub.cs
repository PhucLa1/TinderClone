﻿using Microsoft.AspNetCore.SignalR;

namespace JWTAuthencation.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        //Gửi trạng thái trong khi đang gọi điện
        public async Task CameraStateChange(string userId, bool isCameraOn)
        {
            // Gửi tín hiệu trạng thái camera đến tất cả các thành viên trong phòng
            await Clients.All.SendAsync("CameraState", userId, isCameraOn);
        }

        public async Task AudioStateChange(string userId, bool isAudioOn)
        {
            // Gửi tín hiệu trạng thái camera đến tất cả các thành viên trong phòng
            await Clients.All.SendAsync("AudioState", userId, isAudioOn);
        }

        //Gửi trạng thái trước khi gọi điện
    }
}
