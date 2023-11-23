using Humanizer;
using JWTAuthencation.Models.OtherModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Messaging;

namespace JWTAuthencation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSSendingOrGmailController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public SMSSendingOrGmailController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("SendText")]
        //Sau 30s sẽ phải reset lại cái mã
        public async Task<IActionResult> SendText(string phoneNumber)
        {
            TwilioClient.Init(_configuration["Twilio:AccountSID"], _configuration["Twilio:AuthToken"]);
                var message = MessageResource.Create(
                    body: GenerateOTP(),
                    from: new Twilio.Types.PhoneNumber("+18186394920"),
                    to: new Twilio.Types.PhoneNumber("+84" + phoneNumber)
                );
            int lastIndex = message.Body.LastIndexOf(' ');
            string lastWord = message.Body.Substring(lastIndex + 1);
            return Ok(lastWord);
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
