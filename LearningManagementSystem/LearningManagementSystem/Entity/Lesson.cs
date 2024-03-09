using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LearningManagementSystem.Entity
{
    [Table("Lesson")]
    public class Lesson
    {
        public Lesson()
        {
            LessonResourcess = new HashSet<LessonResources>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LessonId { get; set; }
        [MaxLength(100)]
        public string? SubjectTopicTitle { get; set; }
        //Khóa ngoại
        //many-to-one
        #region
        public int? SubjectTopicId { get; set; }
        [ForeignKey("SubjectTopicId")]
        public SubjectTopic SubjectTopicNavigation { get; set; }
        #endregion
        //one-to-many
        #region
        [JsonIgnore]
        public ICollection<LessonResources> LessonResourcess { get; set; }
        #endregion
    }
}
