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

        #endregion
    }
}
