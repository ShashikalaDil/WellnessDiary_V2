using MessagePack;

namespace WellnessDiaryApi.Data.Dto
{
    public class ArticleDTO
    {
      
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int? CategoryId { get; set; }
        public string Author { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string? ImagePath { get; set; }


    }
}
