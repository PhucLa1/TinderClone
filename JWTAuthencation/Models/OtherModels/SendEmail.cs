namespace JWTAuthencation.Models.OtherModels
{
	public class SendEmail
	{
        public string toEmail { get; set; }
        public string Subject { get; set; }
        public string? Body { get; set; }
    }
}
