using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LearningManagementSystem.Entity
{
    [Table("SubjectTopic")]
    public class SubjectTopic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string SubjectTopicTitle { get; set; }

        //Khóa ngoại
        //many-to-one
        #region
        [JsonIgnore]
        public Guid? SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public Subject SubjectIdNavigation { get; set; }
        #endregion
    }
}
