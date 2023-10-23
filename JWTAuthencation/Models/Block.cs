using System;
using System.Collections.Generic;

namespace JWTAuthencation.Models
{
    public partial class Block
    {
        public int Id { get; set; }
        public byte? OfStatus { get; set; }
        public int? BlockUserId { get; set; }
        public int? BlockedUserId { get; set; }
        public DateTime? BlockDate { get; set; }
    }
}
