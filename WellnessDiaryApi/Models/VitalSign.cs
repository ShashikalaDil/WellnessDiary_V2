using System;
using System.Collections.Generic;

namespace WellnessDiaryApi.Models
{
    public partial class VitalSign
    {
        public int ReadingId { get; set; }
        public int? UserId { get; set; }
        public decimal? Temperature { get; set; }
        public int? RespiratoryRate { get; set; }
        public decimal? OxygenSaturation { get; set; }
        public DateTime? RecordedDateTime { get; set; }

        public virtual User? User { get; set; }
    }
}
