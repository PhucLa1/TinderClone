using Microsoft.EntityFrameworkCore;

namespace JWTAuthencation.Models.ViewModel
{
    [Keyless]
	public class UserRegis
	{
        public string Time { get; set; }
        public int UserCount { get; set; }
    }
}
