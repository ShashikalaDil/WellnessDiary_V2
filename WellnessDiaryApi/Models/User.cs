using System;
using System.Collections.Generic;

namespace WellnessDiaryApi.Models
{
    public partial class User
    {
        public User()
        {
            Appointments = new HashSet<Appointment>();
            BloodPressures = new HashSet<BloodPressure>();
            BloodSugars = new HashSet<BloodSugar>();
            Bmis = new HashSet<Bmi>();
            Cholesterols = new HashSet<Cholesterol>();
            DietNutritions = new HashSet<DietNutrition>();
            EmergencyContacts = new HashSet<EmergencyContact>();
            ExerciseActivities = new HashSet<ExerciseActivity>();
            Fbcs = new HashSet<Fbc>();
            HealthGoals = new HashSet<HealthGoal>();
            HeartRates = new HashSet<HeartRate>();
            MedicalHistories = new HashSet<MedicalHistory>();
            Medications = new HashSet<Medication>();
            Symptoms = new HashSet<Symptom>();
            VitalSigns = new HashSet<VitalSign>();
        }

        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? PasswordHash { get; set; }
        public string Email { get; set; } = null!;
        public string? Name { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? ContactNumber { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string? ProfilePic { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? TokenCreated { get; set; }
        public DateTime? TokenExpires { get; set; }
        public string? PasswordSalt { get; set; }
        public string? Role { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<BloodPressure> BloodPressures { get; set; }
        public virtual ICollection<BloodSugar> BloodSugars { get; set; }
        public virtual ICollection<Bmi> Bmis { get; set; }
        public virtual ICollection<Cholesterol> Cholesterols { get; set; }
        public virtual ICollection<DietNutrition> DietNutritions { get; set; }
        public virtual ICollection<EmergencyContact> EmergencyContacts { get; set; }
        public virtual ICollection<ExerciseActivity> ExerciseActivities { get; set; }
        public virtual ICollection<Fbc> Fbcs { get; set; }
        public virtual ICollection<HealthGoal> HealthGoals { get; set; }
        public virtual ICollection<HeartRate> HeartRates { get; set; }
        public virtual ICollection<MedicalHistory> MedicalHistories { get; set; }
        public virtual ICollection<Medication> Medications { get; set; }
        public virtual ICollection<Symptom> Symptoms { get; set; }
        public virtual ICollection<VitalSign> VitalSigns { get; set; }
    }
}
