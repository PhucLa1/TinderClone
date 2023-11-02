using System.ComponentModel.DataAnnotations.Schema;

namespace JWTAuthencation.Models
{
    public class Admin
    {
        public int? ID { get; set; }
        public byte? OfStatus { get; set; }
        public string UserName { get; set; }
        public string Pass { get; set; }
        
        public string? Token { get; set; }
        public DateTime? TokenCreated { get; set; }
        public DateTime? TokenExpires { get; set; }
    }
}
