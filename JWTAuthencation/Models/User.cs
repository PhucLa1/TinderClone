using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTAuthencation.Models
{
    public partial class User
    {
        public int? Id { get; set; }
        public int? SettingId { get; set; }
        public int? PermissionId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? TagName { get; set; }
        public int? LikeAmount { get; set; }
        public string? Pass { get; set; }
        public string? GoogleId { get; set; }
        public string? FacebookId { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsBlocked { get; set; }
        public bool? IsDeleted { get; set; }
        public string? AboutUser { get; set; }
        public int? PassionId { get; set; }
        public string? JobTitle { get; set; }
        public string? LookingFor { get; set; }
        public bool? Gender { get; set; }
        public string? SexsualOrientation { get; set; }
        public int? Height { get; set; }
        public byte? OfStatus { get; set; }

        //Not in relationship
        [NotMapped]
        public string? AccessToken { get; set; }
    }
}
