using LearningManagementSystem.Migrations;
using Org.BouncyCastle.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LearningManagementSystem.Entity
{
    [Table("ExamAndTestQuestions")]
    public class ExamAndTestQuestions
    {
        public ExamAndTestQuestions()
        {
            ExamAndTestAnswerss = new HashSet<ExamAndTestAnswers>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EaTQuestionId { get; set; }
        [Required]
        [MaxLength(100)]
        public string ExamAndTestQuestionCode { get; set; }
        [Required]
        [MaxLength(255)]
        public string ExamAndTestQuestionContent { get; set; }
        [Required]
        public int Tier { get; set; }
        [Required]
        [MaxLength(100)]
        public string KindOfQuestion { get; set; }
        [Required]
        [MaxLength(50)]
        public string ExamAndTestQuestionType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        //Khóa ngoại
        //many-to-one
        public Guid SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public Subject SubjectNavigation { get; set; }
        public Guid FacultyId { get; set; }
        [ForeignKey("FacultyId")]
        public Faculty FacultyNavigation { get; set; }
        //one-to-many
        public ICollection<ExamAndTestAnswers> ExamAndTestAnswerss { get; set; }
    }
}
