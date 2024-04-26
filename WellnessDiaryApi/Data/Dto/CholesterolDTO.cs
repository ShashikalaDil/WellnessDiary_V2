namespace WellnessDiaryApi.Data.Dto
{
    public class CholesterolDTO
    {
        public int ReadingId { get; set; }
        public int? UserId { get; set; }
        public decimal TotalCholesterol { get; set; }
        public decimal Hdl { get; set; }
        public decimal Ldl { get; set; }
        public DateTime? RecordedDateTime { get; set; }
        public int? StatusId { get; set; }
    }
}
