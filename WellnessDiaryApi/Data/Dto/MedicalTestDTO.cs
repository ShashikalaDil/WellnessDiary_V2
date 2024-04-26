namespace WellnessDiaryApi.Data.Dto
{
    public class MedicalTestDTO
    {
        public int TestId { get; set; }
        public string TestName { get; set; } = null!;
        public int? CategoryId { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
    }
}
