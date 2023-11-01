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
        public string? UserName { get; set; }
        public string? TagName { get; set; }
        public int? LikeAmount { get; set; }
        public string? Pass { get; set; }
        public string? GoogleId { get; set; }
        public string? FacebookId { get; set; }
        public bool? IsBlocked { get; set; }
        public bool? IsDeleted { get; set; }
        public string? AboutUser { get; set; }
        public short? PurposeDateID { get; set; }
        public bool? Gender { get; set; }
        public short? SexsualOrientationID { get; set; }
        public short? Height { get; set; }
        public short? ZodiacID { get; set; }
        public short? EducationID { get; set; }
        public short? FutureFamilyID { get; set; }
        public short? VacxinCovidID { get; set; }
        public short? PersonalityID { get; set; }
        public short? CommunicationID { get; set; }
        public short? LoveLanguageID { get; set; }
        public short? PetID { get; set; }
        public short? AlcolholID { get; set; }
        public short? SmokeID { get; set; }
        public short? WorkoutID { get; set; }
        public short? DietID { get; set; }
        public short? SocialMediaID { get; set; }
        public short? SleepHabitID { get; set; }
        public string? JobTitle { get; set; }
        public string? Company { get; set; }
        public string? School { get; set; }
        public string? LiveAt { get; set; }
        public byte? OfStatus { get; set; }

        //Not in relationship
        
        public string? Token { get; set; }
        public DateTime? TokenCreated { get; set; }
        public DateTime? TokenExpires { get; set; }
    }
}
