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
        public DbSet<QuestionAndAnswer> QuestionAndAnswers { get; set; } = null!;
        public DbSet<QaAFollowers> QaAFollowerss { get; set; } = null!;

        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserClassSubject>()
                .HasKey(ucs => new { ucs.UserId, ucs.ClassId, ucs.SubjectId });
            modelBuilder.Entity<LessonResources>()
                .HasOne(l => l.LessonNavigation)
                .WithMany(lr => lr.LessonResourcess)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<QaAFollowers>()
               .HasOne(l => l.UserIdFollowerNavigation)
               .WithMany(lr => lr.QaAFollowerss)
               .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<QuestionAndAnswer>()
                .HasOne(qa => qa.QaAInOtherQaANavigation)
                .WithMany(qa => qa.QaAInOtherQaAs)
                .HasForeignKey(qa => qa.QaAInOtherQaA)
                .OnDelete(DeleteBehavior.Restrict); // (tuỳ chọn) Nếu bạn không muốn xóa hàng loạt, hãy sử dụng DeleteBehavior.Restrict

            modelBuilder.Entity<QuestionAndAnswer>()
                .HasOne(qa => qa.QaAReplyQaANavigation)
                .WithMany(qa => qa.QaAReplyQaAs)
                .HasForeignKey(qa => qa.QaAReplyQaA)
                .OnDelete(DeleteBehavior.Restrict); // (tuỳ chọn) Nếu bạn không muốn xóa hàng loạt, hãy sử dụng DeleteBehavior.Restrict
        }
    }
}
