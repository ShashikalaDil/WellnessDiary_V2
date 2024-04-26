using System;
using System.Collections.Generic;

namespace WellnessDiaryApi.Models
{
    public partial class HealthGoal
    {
        public int GoalId { get; set; }
        public int? UserId { get; set; }
        public string? GoalType { get; set; }
        public string? TargetMetric { get; set; }
        public int? Progress { get; set; }
        public bool? CompletionStatus { get; set; }

        public virtual User? User { get; set; }
    }
}
