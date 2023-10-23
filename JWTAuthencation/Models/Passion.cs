using System;
using System.Collections.Generic;

namespace JWTAuthencation.Models
{
    public partial class Passion
    {
        public int? Id { get; set; }
        public byte? OfStatus { get; set; }
        public string? Pname { get; set; }
        public string? Descriptions { get; set; }
    }
}
