namespace JWTAuthencation.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Pass { get; set; }
        public string? AccessToken { get; set; }
    }
}
