using System;
using System.Collections.Generic;

namespace JWTAuthencation.Models
{
    public partial class UsersLanguages
    {
        public int Id { get; set; }
        public byte? OfStatus { get; set; }
        public int? LanguageId { get; set; }
        public int? UserId { get; set; }
    }
}
