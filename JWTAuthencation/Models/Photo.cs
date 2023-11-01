using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTAuthencation.Models
{
    public partial class Photo
    {
        public int Id { get; set; }
        public byte? OfStatus { get; set; }
        public string? ImagePath { get; set; }
        public int? UserId { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }
    }
}
