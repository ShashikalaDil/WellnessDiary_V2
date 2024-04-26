namespace WellnessDiaryApi.Data.Dto
{
    public class BmiDTO
    {
        public int ReadingId { get; set; }
        public int? UserId { get; set; }
        public decimal Bmivalue { get; set; }
        public DateTime? RecordedDateTime { get; set; }
        public int? StatusId { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
    }
}
