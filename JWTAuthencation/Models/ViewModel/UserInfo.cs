using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTAuthencation.Models.ViewModel
{

    [Display(Name = "GetUserProfile")]
    public class UserInfo
    {
        public int? ID { get; set; }
        public string? FullName { get; set; }
        public string? TagName { get; set; }
        public int? LikeAmount { get; set; }
        public string? AboutUser { get; set; }
        public int? Age { get; set; }
        public string? PurposeDate { get; set; }
        public bool? Gender { get; set; }

        public string? SexsualOrientation { get; set; }
        public short? Height { get; set; }

        public string? Zodiac { get; set; }

        public string? Education { get; set; }

        public string? FutureFamily { get; set; }

        public string? VacxinCovid { get; set; }

        public string? Personality { get; set; }

        public string? Communication { get; set; }

        public string? LoveLanguage { get; set; }

        public string? Pet { get; set; }

        public string? Alcolhol { get; set; }

        public string? Smoke { get; set; }

        public string? Workout { get; set; }

        public string? Diet { get; set; }

        public string? SocialMedia { get; set; }

        public string? SleepHabit { get; set; }
        public string? JobTitle { get; set; }
        public string? Company { get; set; }
        public string? School { get; set; }
        public string? LiveAt { get; set; }
        [NotMapped]public List<string>? passion { get; set; }
        [NotMapped] public List<string>? languages { get; set; }
        [NotMapped] public List<string>? photos { get; set; }
    }
}
