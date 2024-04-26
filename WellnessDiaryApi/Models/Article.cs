using System;
using System.Collections.Generic;

namespace WellnessDiaryApi.Models
{
    public partial class Article
    {
        public int ArticleId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int? CategoryId { get; set; }
        public string? Author { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string? ImagePath { get; set; }

        public virtual ArticleCategory? Category { get; set; }
    }
}
