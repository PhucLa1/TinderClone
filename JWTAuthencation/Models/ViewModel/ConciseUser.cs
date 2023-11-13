namespace JWTAuthencation.Models.ViewModel
{
	public class ConciseUser
	{
		public double Latitude { get; set; }
		public double Longtitude { get; set; }
		public int Age { get; set; }
		public int AgeMax { get; set; }
		public int AgeMin { get; set; }
		public int MaxDistance { get; set; }
		public bool Gender { get; set; }
		public int SexO { get; set; }
		public bool GlobalMatch { get; set; }
		public HashSet<int> Language { get; set; }
	}
}
