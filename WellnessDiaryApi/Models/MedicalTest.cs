using System;
using System.Collections.Generic;

namespace WellnessDiaryApi.Models
{
    public partial class MedicalTest
    {
        public int TestId { get; set; }
        public string TestName { get; set; } = null!;
        public int? CategoryId { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }

        public virtual MedicalTestCategory? Category { get; set; }
    }
}
