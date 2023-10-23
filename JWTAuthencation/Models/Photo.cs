using System;
using System.Collections.Generic;

namespace JWTAuthencation.Models
{
    public partial class Photo
    {
        public int Id { get; set; }
        public byte? OfStatus { get; set; }
        public string? ImagePath { get; set; }
        public int? UserId { get; set; }
    }
}
