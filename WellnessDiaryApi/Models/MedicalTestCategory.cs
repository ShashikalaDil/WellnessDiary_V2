using System;
using System.Collections.Generic;

namespace WellnessDiaryApi.Models
{
    public partial class MedicalTestCategory
    {
        public MedicalTestCategory()
        {
            MedicalTests = new HashSet<MedicalTest>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public virtual ICollection<MedicalTest> MedicalTests { get; set; }
    }
}
