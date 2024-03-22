using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningManagementSystem.Entity
{
    [Table("LearningStatistics")]
    public class LearningStatistics
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public Guid StudentId { get; set; }
        [Required]
        public Guid ClassId { get; set; }
        [Required]
        public Guid SubjectId { get; set; }
        public DateTime? TimeBegin { get; set; }
        public string DayOfWeek { get; set; }
        public DateTime? TimeEnd { get; set; }
        public double? totalMinutes { get; set; }

        [ForeignKey("StudentId")]
        public User StudentNavigation { get; set; }
        [ForeignKey("ClassId")]
        public Class ClassNavigation { get; set; }
        [ForeignKey("SubjectId")]
        public Subject SubjectNavigation { get; set; }
    }
}
