using System;
using System.Collections.Generic;

namespace WellnessDiaryApi.Models
{
    public partial class Bmi
    {
        public int ReadingId { get; set; }
        public int? UserId { get; set; }
        public decimal Bmivalue { get; set; }
        public DateTime? RecordedDateTime { get; set; }
        public int? StatusId { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }

        public virtual User? User { get; set; }
    }
}
