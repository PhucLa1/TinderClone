using System;
using System.Collections.Generic;

namespace JWTAuthencation.Models
{
    public partial class Mess
    {
        public int Id { get; set; }
        public byte? OfStatus { get; set; }
        public int? SendUserId { get; set; }
        public int? ReceiveUserId { get; set; }
        public string? Content { get; set; }
        public DateTime? SendTime { get; set; }
    }
}
