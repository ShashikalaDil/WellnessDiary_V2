using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WellnessDiaryApi.Models;

namespace WellnessDiaryApi.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appointment> Appointments { get; set; } = null!;
        public virtual DbSet<Article> Articles { get; set; } = null!;
        public virtual DbSet<ArticleCategory> ArticleCategories { get; set; } = null!;
        public virtual DbSet<BloodPressure> BloodPressures { get; set; } = null!;
        public virtual DbSet<BloodSugar> BloodSugars { get; set; } = null!;
        public virtual DbSet<Bmi> Bmis { get; set; } = null!;
        public virtual DbSet<Cholesterol> Cholesterols { get; set; } = null!;
        public virtual DbSet<DietNutrition> DietNutritions { get; set; } = null!;
        public virtual DbSet<EmergencyContact> EmergencyContacts { get; set; } = null!;
        public virtual DbSet<ExerciseActivity> ExerciseActivities { get; set; } = null!;
        public virtual DbSet<Fbc> Fbcs { get; set; } = null!;
        public virtual DbSet<HealthGoal> HealthGoals { get; set; } = null!;
        public virtual DbSet<HeartRate> HeartRates { get; set; } = null!;
        public virtual DbSet<MedicalHistory> MedicalHistories { get; set; } = null!;
        public virtual DbSet<MedicalTest> MedicalTests { get; set; } = null!;
        public virtual DbSet<MedicalTestCategory> MedicalTestCategories { get; set; } = null!;
        public virtual DbSet<Medication> Medications { get; set; } = null!;
        public virtual DbSet<News> News { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<Symptom> Symptoms { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<VitalSign> VitalSigns { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;Database=WellnessDBv2;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");

                entity.Property(e => e.AppointmentDateTime).HasColumnType("datetime");

                entity.Property(e => e.HealthcareProvider)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Notes).IsUnicode(false);

                entity.Property(e => e.Reason).IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Appointme__UserI__3B75D760");
            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.Property(e => e.ArticleId).HasColumnName("ArticleID");

                entity.Property(e => e.Author)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.Content).IsUnicode(false);

                entity.Property(e => e.ImagePath).IsUnicode(false);

                entity.Property(e => e.PublishedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Articles__Catego__6383C8BA");
            });

            modelBuilder.Entity<ArticleCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PK__ArticleC__19093A2B4D7AA748");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BloodPressure>(entity =>
            {
                entity.HasKey(e => e.ReadingId)
                    .HasName("PK__BloodPre__C80F9C6EEA7ED507");

                entity.ToTable("BloodPressure");

                entity.Property(e => e.ReadingId).HasColumnName("ReadingID");

                entity.Property(e => e.Diastolic).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.RecordedDateTime).HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.Systolic).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BloodPressures)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__BloodPres__UserI__1ED998B2");
            });

            modelBuilder.Entity<BloodSugar>(entity =>
            {
                entity.HasKey(e => e.ReadingId)
                    .HasName("PK__BloodSug__C80F9C6E39118D46");

                entity.ToTable("BloodSugar");

                entity.Property(e => e.ReadingId).HasColumnName("ReadingID");

                entity.Property(e => e.BloodSugarLevel).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.RecordedDateTime).HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BloodSugars)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__BloodSuga__UserI__21B6055D");
            });

            modelBuilder.Entity<Bmi>(entity =>
            {
                entity.HasKey(e => e.ReadingId)
                    .HasName("PK__BMI__C80F9C6E253518C2");

                entity.ToTable("BMI");

                entity.Property(e => e.ReadingId).HasColumnName("ReadingID");

                entity.Property(e => e.Bmivalue)
                    .HasColumnType("decimal(8, 2)")
                    .HasColumnName("BMIValue");

                entity.Property(e => e.Height).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.RecordedDateTime).HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Weight).HasColumnType("decimal(8, 2)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Bmis)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__BMI__UserID__2A4B4B5E");
            });

            modelBuilder.Entity<Cholesterol>(entity =>
            {
                entity.HasKey(e => e.ReadingId)
                    .HasName("PK__Choleste__C80F9C6E9DDE8BA6");

                entity.ToTable("Cholesterol");

                entity.Property(e => e.ReadingId).HasColumnName("ReadingID");

                entity.Property(e => e.Hdl)
                    .HasColumnType("decimal(8, 2)")
                    .HasColumnName("HDL");

                entity.Property(e => e.Ldl)
                    .HasColumnType("decimal(8, 2)")
                    .HasColumnName("LDL");

                entity.Property(e => e.RecordedDateTime).HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.TotalCholesterol).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Cholesterols)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Cholester__UserI__276EDEB3");
            });

            modelBuilder.Entity<DietNutrition>(entity =>
            {
                entity.HasKey(e => e.NutritionId)
                    .HasName("PK__DietNutr__8A74A1B685E8C188");

                entity.ToTable("DietNutrition");

                entity.Property(e => e.NutritionId).HasColumnName("NutritionID");

                entity.Property(e => e.Carbohydrates).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Fat).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.MealName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Protein).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.RecordedDateTime).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DietNutritions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__DietNutri__UserI__46E78A0C");
            });

            modelBuilder.Entity<EmergencyContact>(entity =>
            {
                entity.HasKey(e => e.ContactId)
                    .HasName("PK__Emergenc__5C6625BB048A1703");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.ContactName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ContactNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Relationship)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EmergencyContacts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Emergency__UserI__1273C1CD");
            });

            modelBuilder.Entity<ExerciseActivity>(entity =>
            {
                entity.HasKey(e => e.ActivityId)
                    .HasName("PK__Exercise__45F4A7F125B785DE");

                entity.ToTable("ExerciseActivity");

                entity.Property(e => e.ActivityId).HasColumnName("ActivityID");

                entity.Property(e => e.ActivityType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IntensityLevel)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RecordedDateTime).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ExerciseActivities)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__ExerciseA__UserI__49C3F6B7");
            });

            modelBuilder.Entity<Fbc>(entity =>
            {
                entity.HasKey(e => e.ReadingId)
                    .HasName("PK__FBC__C80F9C6EEAE8B8EB");

                entity.ToTable("FBC");

                entity.Property(e => e.ReadingId).HasColumnName("ReadingID");

                entity.Property(e => e.Eosinophils).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.Hemoglobin).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.Lymphocytes).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.Neutrophils).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.PlateletCount).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.Rbc)
                    .HasColumnType("decimal(8, 2)")
                    .HasColumnName("RBC");

                entity.Property(e => e.RecordedDateTime).HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.WhiteBloodCellCount).HasColumnType("decimal(8, 2)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Fbcs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__FBC__UserID__2D27B809");
            });

            modelBuilder.Entity<HealthGoal>(entity =>
            {
                entity.HasKey(e => e.GoalId)
                    .HasName("PK__HealthGo__8A4FFF31970431B6");

                entity.Property(e => e.GoalId).HasColumnName("GoalID");

                entity.Property(e => e.GoalType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TargetMetric)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HealthGoals)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__HealthGoa__UserI__4CA06362");
            });

            modelBuilder.Entity<HeartRate>(entity =>
            {
                entity.HasKey(e => e.ReadingId)
                    .HasName("PK__HeartRat__C80F9C6E2D8D3915");

                entity.ToTable("HeartRate");

                entity.Property(e => e.ReadingId).HasColumnName("ReadingID");

                entity.Property(e => e.HeartRate1).HasColumnName("HeartRate");

                entity.Property(e => e.RecordedDateTime).HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HeartRates)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__HeartRate__UserI__24927208");
            });

            modelBuilder.Entity<MedicalHistory>(entity =>
            {
                entity.HasKey(e => e.HistoryId)
                    .HasName("PK__MedicalH__4D7B4ADDD804A713");

                entity.ToTable("MedicalHistory");

                entity.Property(e => e.HistoryId).HasColumnName("HistoryID");

                entity.Property(e => e.Allergies).IsUnicode(false);

                entity.Property(e => e.Diagnosis)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FamilyMedicalHistory).IsUnicode(false);

                entity.Property(e => e.Surgery)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MedicalHistories)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__MedicalHi__UserI__412EB0B6");
            });

            modelBuilder.Entity<MedicalTest>(entity =>
            {
                entity.HasKey(e => e.TestId)
                    .HasName("PK__MedicalT__8CC33100845B87C1");

                entity.Property(e => e.TestId).HasColumnName("TestID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.ImagePath).IsUnicode(false);

                entity.Property(e => e.TestName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.MedicalTests)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__MedicalTe__Categ__173876EA");
            });

            modelBuilder.Entity<MedicalTestCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PK__MedicalT__19093A2BB43BD08E");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Medication>(entity =>
            {
                entity.Property(e => e.MedicationId).HasColumnName("MedicationID");

                entity.Property(e => e.Dosage)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.Frequency)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MedicationName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Medications)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Medicatio__UserI__38996AB5");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.NewsCategory)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NewsContent).IsUnicode(false);

                entity.Property(e => e.NewsHeader)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NewsImagePath).IsUnicode(false);

                entity.Property(e => e.NewsPublishDate).HasColumnType("date");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.Status1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Status");

                entity.Property(e => e.StatusDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Symptom>(entity =>
            {
                entity.Property(e => e.SymptomId).HasColumnName("SymptomID");

                entity.Property(e => e.Notes).IsUnicode(false);

                entity.Property(e => e.RecordedDateTime).HasColumnType("datetime");

                entity.Property(e => e.SymptomName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Symptoms)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Symptoms__UserID__3E52440B");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ContactNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash).IsUnicode(false);

                entity.Property(e => e.PasswordSalt).IsUnicode(false);

                entity.Property(e => e.ProfilePic)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.RefreshToken).IsUnicode(false);

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");

                entity.Property(e => e.Role)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TokenCreated).HasColumnType("date");

                entity.Property(e => e.TokenExpires).HasColumnType("date");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VitalSign>(entity =>
            {
                entity.HasKey(e => e.ReadingId)
                    .HasName("PK__VitalSig__C80F9C6EAC047011");

                entity.Property(e => e.ReadingId).HasColumnName("ReadingID");

                entity.Property(e => e.OxygenSaturation).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.RecordedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Temperature).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.VitalSigns)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__VitalSign__UserI__440B1D61");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
