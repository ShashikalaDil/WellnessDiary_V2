namespace WellnessDiaryApi.Data.Dto
{
    public class MedicalHistoryDTO
    {
        public int HistoryId { get; set; }
        public int? UserId { get; set; }
        public string Diagnosis { get; set; } = null!;
        public string? Surgery { get; set; }
        public string? Allergies { get; set; }
        public string? FamilyMedicalHistory { get; set; }
    }
}
