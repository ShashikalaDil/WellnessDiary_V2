using System;
using System.Collections.Generic;

namespace WellnessDiaryApi.Models
{
    public partial class BloodPressure
    {
        public int ReadingId { get; set; }
        public int? UserId { get; set; }
        public decimal Systolic { get; set; }
        public decimal Diastolic { get; set; }
        public DateTime? RecordedDateTime { get; set; }
        public int? StatusId { get; set; }

        public virtual User? User { get; set; }
    }
}
