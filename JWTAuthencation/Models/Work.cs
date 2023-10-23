using System;
using System.Collections.Generic;

namespace JWTAuthencation.Models
{
    public partial class Work
    {
        public int Id { get; set; }
        public byte? OfStatus { get; set; }
        public string? Wname { get; set; }
        public string? Descriptions { get; set; }
    }
}
