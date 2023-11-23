namespace JWTAuthencation.Models.ViewModel
{
    public class DetailRecommend
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string LiveAt { get; set; }
        public int Age { get; set; }
        public List<string> ImagePath { get; set; }
        public double Distance { get; set; }

    }
}
