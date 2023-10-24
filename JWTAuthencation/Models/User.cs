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
        public int? PurposeDateID { get; set; }
        public bool? Gender { get; set; }
        public int? SexsualOrientationID { get; set; }
        public int? Height { get; set; }
        public int? ZodiacID { get; set; }
        public int? EducationID { get; set; }
        public int? FutureFamilyID { get; set; }
        public int? VacxinCovidID { get; set; }
        public int? PersonalityID { get; set; }
        public int? CommunicationID { get; set; }
        public int? LoveLanguageID { get; set; }
        public int? PetID { get; set; }
        public int? AlcolholID { get; set; }
        public int? SmokeID { get; set; }
        public int? WorkoutID { get; set; }
        public int? DietID { get; set; }
        public int? SocialMediaID { get; set; }
        public int? SleepHabitID { get; set; }
        public string? JobTitle { get; set; }
        public string? Company { get; set; }
        public string? School { get; set; }
        public string? LiveAt { get; set; }
        public int? OfStatus { get; set; }

        //Not in relationship
        [NotMapped]
        public string? AccessToken { get; set; }
    }
}
