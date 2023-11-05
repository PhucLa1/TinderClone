using System.Drawing;

namespace JWTAuthencation.Models
{
    public class ReportAccount
    {
        public int? ID { get; set; }
        public int? ReportedUserID { get; set; }
        public int? IdReportFrom { get; set; }
        public string? Reason { get; set; }
        public byte? OfStatus { get; set; }
    }
}
