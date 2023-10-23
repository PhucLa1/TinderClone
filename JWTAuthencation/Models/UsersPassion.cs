using System;
using System.Collections.Generic;

namespace JWTAuthencation.Models
{
    public partial class UsersPassion
    {
        public int Id { get; set; }
        public byte? OfStatus { get; set; }
        public int? PassionId { get; set; }
        public int? UserId { get; set; }
    }
}
