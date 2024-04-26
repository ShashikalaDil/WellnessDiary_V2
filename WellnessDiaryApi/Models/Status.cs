using System;
using System.Collections.Generic;

namespace WellnessDiaryApi.Models
{
    public partial class Status
    {
        public int StatusId { get; set; }
        public string Status1 { get; set; } = null!;
        public string? StatusDescription { get; set; }
    }
}
