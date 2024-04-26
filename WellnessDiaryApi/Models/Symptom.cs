using System;
using System.Collections.Generic;

namespace WellnessDiaryApi.Models
{
    public partial class Symptom
    {
        public int SymptomId { get; set; }
        public int? UserId { get; set; }
        public string SymptomName { get; set; } = null!;
        public int? Severity { get; set; }
        public DateTime? RecordedDateTime { get; set; }
        public string? Notes { get; set; }

        public virtual User? User { get; set; }
    }
}
