using System;
using System.Collections.Generic;

namespace WellnessDiaryApi.Models
{
    public partial class MedicalHistory
    {
        public int HistoryId { get; set; }
        public int? UserId { get; set; }
        public string Diagnosis { get; set; } = null!;
        public string? Surgery { get; set; }
        public string? Allergies { get; set; }
        public string? FamilyMedicalHistory { get; set; }

        public virtual User? User { get; set; }
    }
}
