namespace WellnessDiaryApi.Data.Dto
{
    public class BloodSugarDTO
    {
        public int ReadingId { get; set; }
        public int? UserId { get; set; }
        public decimal BloodSugarLevel { get; set; }
        public DateTime? RecordedDateTime { get; set; }
        public int? StatusId { get; set; }
    }
}
