using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int EaTQuestionId { get; set; }
        [ForeignKey("EaTQuestionId")]
        public ExamAndTestQuestions ExamAndTestQuestionsNavigation { get; set; }
    }
}
