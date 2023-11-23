using System;
using System.Collections.Generic;

namespace JWTAuthencation.Models
{
    public partial class Call
    {
        public int? Id { get; set; }
        public byte? OfStatus { get; set; }
        public int? CallerId { get; set; }
        public int? ReceiverId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public short? Duration { get; set; }
        public int? CallStatusId { get; set; }
    }
}
