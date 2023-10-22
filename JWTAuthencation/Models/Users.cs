using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTAuthencation.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Pass { get; set; }
        public string? AccessToken { get; set; }
        public string? ImagePath { get; set; }


        [NotMapped]
        public IFormFile? FileImage { get; set; }
    }
}
