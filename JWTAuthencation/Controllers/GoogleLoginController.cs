using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JWTAuthencation.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class GoogleLoginController : ControllerBase
    {
        [HttpGet("google")]
        public async Task<IActionResult> LoginWithGoogle()
        {
            // Gọi Challenge để chuyển hướng người dùng đến trang đăng nhập Google.
            // GoogleDefaults.AuthenticationScheme là tên của mặc định AuthenticationScheme cho Google.
            return Challenge(new AuthenticationProperties ());
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (result.Succeeded)
            {
                // Đăng nhập thành công, bạn có thể thực hiện các tác vụ khác ở đây.
                // result.Principal chứa thông tin người dùng đăng nhập.

                // Lấy thông tin người dùng
                var user = result.Principal;

                // Lấy email người dùng (ví dụ)
                var userEmail = user.FindFirst(ClaimTypes.Email)?.Value;

                // Lưu thông tin người dùng vào phiên làm việc hoặc tạo token JWT

                // Chuyển hướng hoặc trả về phản hồi tùy theo nhu cầu của bạn
                return Ok(userEmail);
            }
            else
            {
                // Đăng nhập không thành công, xử lý theo nhu cầu của bạn
                return BadRequest("Đăng nhập không thành công.");
            }
        }
    }
}
