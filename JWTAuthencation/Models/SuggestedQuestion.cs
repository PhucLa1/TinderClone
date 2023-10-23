using System;
using System.Collections.Generic;

namespace JWTAuthencation.Models
{
    public partial class SuggestedQuestion
    {
        public int Id { get; set; }
        public byte? OfStatus { get; set; }
        public string? Ques { get; set; }
    }
}
