namespace JWTAuthencation.Models
{
    public class ViolationSuspension
    {
        public int? ID { get; set; }
        public int? UserID { get; set; }
        public int? ReportID { get; set; }
        public DateTime Suspended_at { get; set; }
        public byte? OfStatus { get; set; }
    }
}
