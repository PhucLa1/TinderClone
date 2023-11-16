namespace JWTAuthencation.Models.ViewModel
{
	public class ConciseUser
	{
        public bool Gender { get; set; }
        public int SexO { get; set; }
        public int Age { get; set; }
        public int AgeMax { get; set; }
        public int AgeMin { get; set; }
        public HashSet<int> Language { get; set; }
        public int Global { get; set; }
        public double Latitude { get; set; }
        public double Longtitude { get; set; }
        public int MaxDistance { get; set; }
    }
}
