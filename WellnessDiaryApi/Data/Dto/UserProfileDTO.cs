namespace WellnessDiaryApi.Data.Dto
{
    public class UserProfileDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        
        public string Email { get; set; } = null!;
        public string? Name { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? ContactNumber { get; set; }
    }
}
