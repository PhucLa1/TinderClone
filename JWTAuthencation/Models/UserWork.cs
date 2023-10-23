using System;
using System.Collections.Generic;

namespace JWTAuthencation.Models
{
    public partial class UserWork
    {
        public int Id { get; set; }
        public byte? OfStatus { get; set; }
        public int? UserId { get; set; }
        public int? WorkId { get; set; }
    }
}
