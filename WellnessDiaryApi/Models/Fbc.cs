using System;
using System.Collections.Generic;

namespace WellnessDiaryApi.Models
{
    public partial class Fbc
    {
        public int ReadingId { get; set; }
        public int? UserId { get; set; }
        public decimal? Hemoglobin { get; set; }
        public decimal? WhiteBloodCellCount { get; set; }
        public decimal? PlateletCount { get; set; }
        public DateTime? RecordedDateTime { get; set; }
        public int StatusId { get; set; }
        public decimal? Rbc { get; set; }
        public decimal? Neutrophils { get; set; }
        public decimal? Eosinophils { get; set; }
        public decimal? Lymphocytes { get; set; }

        public virtual User? User { get; set; }
    }
}
