using Microsoft.EntityFrameworkCore;

namespace JWTAuthencation.Models.ViewModel
{
    [Keyless]
    public class MessCount
    {
        public int Hour { get; set; }
        public int MessageCount { get; set; }
    }
}
