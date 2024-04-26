using System;
using System.Collections.Generic;

namespace WellnessDiaryApi.Models
{
    public partial class Cholesterol
    {
        public int ReadingId { get; set; }
        public int? UserId { get; set; }
        public decimal TotalCholesterol { get; set; }
        public decimal Hdl { get; set; }
        public decimal Ldl { get; set; }
        public DateTime? RecordedDateTime { get; set; }
        public int? StatusId { get; set; }

        public virtual User? User { get; set; }
    }
}
