using System;
using System.Collections.Generic;

namespace WellnessDiaryApi.Models
{
    public partial class ExerciseActivity
    {
        public int ActivityId { get; set; }
        public int? UserId { get; set; }
        public string? ActivityType { get; set; }
        public int? DurationMinutes { get; set; }
        public string? IntensityLevel { get; set; }
        public DateTime? RecordedDateTime { get; set; }

        public virtual User? User { get; set; }
    }
}
