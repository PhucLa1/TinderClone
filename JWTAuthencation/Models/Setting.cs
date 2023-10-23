using System;
using System.Collections.Generic;

namespace JWTAuthencation.Models
{
    public partial class Setting
    {
        public int Id { get; set; }
        public byte? OfStatus { get; set; }
        public double? Latitute { get; set; }
        public double? Longtitute { get; set; }
        public int? DistancePreference { get; set; }
        public string? LookFor { get; set; }
        public int? AgeMin { get; set; }
        public int? AgeMax { get; set; }
        public int? DistanceUnit { get; set; }
        public int? GlobalMatches { get; set; }
        public int? HideAge { get; set; }
        public int? HideDistance { get; set; }
    }
}
