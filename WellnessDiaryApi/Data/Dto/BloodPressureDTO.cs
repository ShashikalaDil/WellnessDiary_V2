namespace WellnessDiaryApi.Data.Dto
{
    public class BloodPressureDTO
    {
        public int ReadingId { get; set; }
        public int? UserId { get; set; }
        public decimal Systolic { get; set; }
        public decimal Diastolic { get; set; }
        public DateTime? RecordedDateTime { get; set; }
        public int? StatusId { get; set; }
    }
}
