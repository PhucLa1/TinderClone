using System;
using System.Collections.Generic;

namespace JWTAuthencation.Models
{
    public partial class Permission
    {
        public int Id { get; set; }
        public byte? OfStatus { get; set; }
        public string? PerName { get; set; }
        public string? RoleDetails { get; set; }
    }
}
