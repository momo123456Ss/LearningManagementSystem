using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LearningManagementSystem.Entity
{
    [Table("ExamAndTestAnswers")]
    public class ExamAndTestAnswers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string AnswerContent { get; set; }
        [Required]
        public bool isAnswer {  get; set; }

        //Khóa ngoại
        //many-to-one
        [JsonIgnore]
        public int EaTQuestionId { get; set; }
        [JsonIgnore]
        [ForeignKey("EaTQuestionId")]
        public ExamAndTestQuestions ExamAndTestQuestionsNavigation { get; set; }
    }
}
