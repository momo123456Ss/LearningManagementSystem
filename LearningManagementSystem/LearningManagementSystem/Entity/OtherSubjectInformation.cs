using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LearningManagementSystem.Entity
{
    [Table("OtherSubjectInformation")]
    public class OtherSubjectInformation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string OtherSubjectTitle { get; set; }
        [Required]
        [MaxLength(255)]
        public string OtherSubjectDescription{ get; set; }

        //Khóa ngoại
        //many-to-one
        #region
        [JsonIgnore]
        public Guid? SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public Subject SubjectNavigation { get; set; }
        #endregion
    }
}
