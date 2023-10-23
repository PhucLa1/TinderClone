using System;
using System.Collections.Generic;

namespace JWTAuthencation.Models
{
    public partial class CallStatus
    {
        public int Id { get; set; }
        public byte? OfStatus { get; set; }
        public string? Csname { get; set; }
    }
}
