namespace WellnessDiaryApi.Data.Dto
{
    public class FbcDTO
    {
        public int ReadingId { get; set; }
        public int? UserId { get; set; }
        public decimal? Hemoglobin { get; set; }
        public decimal? WhiteBloodCellCount { get; set; }
        public decimal? PlateletCount { get; set; }
        public DateTime? RecordedDateTime { get; set; }       
        public decimal? Rbc { get; set; }
        public decimal? Neutrophils { get; set; }
        public decimal? Eosinophils { get; set; }
        public decimal? Lymphocytes { get; set; }

    }
}
