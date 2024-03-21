using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Models.ExamAndTestQuestionModel.AnswerModel
{
    public class ExamAndTestAnswerCreateModel
    {
        [Required]
        [MaxLength(255)]
        public string AnswerContent { get; set; }
        [Required]
        public bool isAnswer { get; set; }
    }
    public class ExamAndTestAnswerAddOrUpdateModel
    {
        [MaxLength(255)]
        public string? AnswerContent { get; set; }
        public bool? isAnswer { get; set; }
        public int? EaTQuestionId { get; set; }

    }
    public class ExamAndTestAnswerViewModel
    {
        public int Id { get; set; }
        public string AnswerContent { get; set; }
        public bool isAnswer { get; set; }

    }

}
