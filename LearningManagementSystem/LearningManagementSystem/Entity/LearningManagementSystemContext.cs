using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Entity
{
    public class LearningManagementSystemContext : DbContext
    {
        public LearningManagementSystemContext(DbContextOptions options) : base(options) { }
        #region DbSet
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public DbSet<CloudinaryConfiguration> CloudinaryConfigurations { get; set; } = null!;
        public DbSet<MailConfiguration> MailConfigurations { get; set; } = null!;
        public DbSet<Class> Classs { get; set; } = null!;
        public DbSet<Faculty> Facultys { get; set; } = null!;
        public DbSet<UserBelongToFaculty> UserBelongToFacultys { get; set; } = null!;
        public DbSet<OtherSubjectInformation> OtherSubjectInformations { get; set; } = null!;
        public DbSet<Subject> Subjects { get; set; } = null!;
        public DbSet<SubjectTopic> SubjectTopics { get; set; } = null!;
        public DbSet<UserClassSubject> UserClassSubjects { get; set; } = null!;
        public DbSet<LecturesAndResources> LecturesAndResourcesL { get; set; } = null!;
        public DbSet<Lesson> Lessons { get; set; } = null!;
        public DbSet<LessonResources> LessonResourcess { get; set; } = null!;


        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserClassSubject>()
                .HasKey(ucs => new { ucs.UserId, ucs.ClassId, ucs.SubjectId });
            modelBuilder.Entity<LessonResources>()
                .HasOne(l => l.LessonNavigation)
                .WithMany(lr => lr.LessonResourcess)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
