using System;
using System.Collections.Generic;

namespace WellnessDiaryApi.Models
{
    public partial class News
    {
        public int NewsId { get; set; }
        public string? NewsHeader { get; set; }
        public DateTime? NewsPublishDate { get; set; }
        public string? NewsContent { get; set; }
        public string? NewsCategory { get; set; }
        public string? NewsImagePath { get; set; }
    }
}
