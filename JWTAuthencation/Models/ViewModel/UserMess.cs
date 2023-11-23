namespace JWTAuthencation.Models.ViewModel
{
	public class UserMess
	{
		public int? UserID { get; set; }
		public string? ImagePath { get; set; }
		public string? UserName { get; set; }
        public string? LastMess { get; set; }
        public int? LastUserChat { get; set; }
        public DateTime? SendTime { get; set; }
    }
}
