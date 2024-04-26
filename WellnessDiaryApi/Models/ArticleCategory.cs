using System;
using System.Collections.Generic;

namespace WellnessDiaryApi.Models
{
    public partial class ArticleCategory
    {
        public ArticleCategory()
        {
            Articles = new HashSet<Article>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public virtual ICollection<Article> Articles { get; set; }
    }
}
