namespace WellnessDiaryApi.Data.Dto
{
    public class MedicationDTO
    {
        public int MedicationId { get; set; }
        public int? UserId { get; set; }
        public string MedicationName { get; set; } = null!;
        public string? Dosage { get; set; }
        public string? Frequency { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
