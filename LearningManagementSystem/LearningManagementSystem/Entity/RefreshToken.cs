using System.ComponentModel.DataAnnotations.Schema;

namespace LearningManagementSystem.Entity
{
    [Table("RefreshToken")]
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; } = null!;
        public string JwtId { get; set; } = null!;
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiredAt { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
    }
}
