namespace WellnessDiaryApi.Data.Dto
{
    public class SymptomDTO
    {
        public int SymptomId { get; set; }
        public int? UserId { get; set; }
        public string SymptomName { get; set; } = null!;
        public int? Severity { get; set; }
        public DateTime? RecordedDateTime { get; set; }
        public string? Notes { get; set; }
    }
}
