using System;
using System.Collections.Generic;

namespace JWTAuthencation.Models
{
    public partial class Like
    {
        public int Id { get; set; }
        public byte? OfStatus { get; set; }
        public int LikeUserId { get; set; }
        public int LikedUserId { get; set; }
        public DateTime? LikeDate { get; set; }
        public bool? Matches { get; set; }
    }
}
