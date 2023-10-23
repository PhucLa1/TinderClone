using System;
using System.Collections.Generic;

namespace JWTAuthencation.Models
{
    public partial class Unlike
    {
        public int Id { get; set; }
        public byte? OfStatus { get; set; }
        public int? UnlikeUserId { get; set; }
        public int? UnlikedUserId { get; set; }
        public DateTime? UnlikeDate { get; set; }
    }
}
