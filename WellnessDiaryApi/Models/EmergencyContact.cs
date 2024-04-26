using System;
using System.Collections.Generic;

namespace WellnessDiaryApi.Models
{
    public partial class EmergencyContact
    {
        public int ContactId { get; set; }
        public int? UserId { get; set; }
        public string? ContactName { get; set; }
        public string? Relationship { get; set; }
        public string? ContactNumber { get; set; }

        public virtual User? User { get; set; }
    }
}
