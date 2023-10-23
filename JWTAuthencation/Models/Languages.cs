using System;
using System.Collections.Generic;

namespace JWTAuthencation.Models
{
    public partial class Languages
    {
        public int? Id { get; set; }
        public byte? OfStatus { get; set; }
        public string? Lname { get; set; }
        public string? Descriptions { get; set; }
    }
}
