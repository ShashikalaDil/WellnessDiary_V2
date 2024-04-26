using System;
using System.Collections.Generic;

namespace WellnessDiaryApi.Models
{
    public partial class Appointment
    {
        public int AppointmentId { get; set; }
        public int? UserId { get; set; }
        public DateTime? AppointmentDateTime { get; set; }
        public string? HealthcareProvider { get; set; }
        public string? Reason { get; set; }
        public string? Notes { get; set; }

        public virtual User? User { get; set; }
    }
}
