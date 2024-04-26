using System;
using System.Collections.Generic;

namespace WellnessDiaryApi.Models
{
    public partial class DietNutrition
    {
        public int NutritionId { get; set; }
        public int? UserId { get; set; }
        public string? MealName { get; set; }
        public int? Calories { get; set; }
        public decimal? Protein { get; set; }
        public decimal? Carbohydrates { get; set; }
        public decimal? Fat { get; set; }
        public DateTime? RecordedDateTime { get; set; }

        public virtual User? User { get; set; }
    }
}
