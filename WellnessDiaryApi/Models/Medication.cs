using System;
using System.Collections.Generic;

namespace WellnessDiaryApi.Models
{
    public partial class Medication
    {
        public int MedicationId { get; set; }
        public int? UserId { get; set; }
        public string MedicationName { get; set; } = null!;
        public string? Dosage { get; set; }
        public string? Frequency { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual User? User { get; set; }
    }
}
