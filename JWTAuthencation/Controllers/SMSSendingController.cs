using JWTAuthencation.Models.OtherModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace JWTAuthencation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSSendingController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public SMSSendingController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("SendText")]
        public async Task<IActionResult> SendText(string phoneNumber)
        {
            TwilioClient.Init(_configuration["Twilio:AccountSID"], _configuration["Twilio:AuthToken"]);

            var message = MessageResource.Create(
                body: GenerateOTP(),
                from: new Twilio.Types.PhoneNumber("+18186394920"),
                to: new Twilio.Types.PhoneNumber("+84" + phoneNumber)
            );

            return Ok("return successfully " + message.Sid + "content is "+ message.Body);
        }

        private string GenerateOTP()
        {
            
            string secretKey = "GQDSF4ORUIP2VF75";
            long counter = DateTime.UtcNow.Ticks / 30000; // 30 seconds time step
            byte[] counterBytes = BitConverter.GetBytes(counter);
            byte[] keyBytes = Base32Encoding.ToBytes(secretKey);

            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                byte[] hash = hmacsha256.ComputeHash(counterBytes);
                int offset = hash[hash.Length - 1] & 0x0F;
                int binary = (hash[offset] & 0x7F) << 24 | (hash[offset + 1] & 0xFF) << 16 | (hash[offset + 2] & 0xFF) << 8 | (hash[offset + 3] & 0xFF);
                int otp = binary % 1000000; // 6-digit OTP
                return otp.ToString("D6");
            }
        }      
    }
}
