using LearningManagementSystem.Models.ExamAndTestQuestionModel.AnswerModel;
using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Models.ExamAndTestQuestionModel
{
    public class ExamAndTestQuestionCreateModel
    {
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
        [Required]
        public Guid SubjectId { get; set; }
        [Required]
        public Guid FacultyId { get; set; }
        public ICollection<ExamAndTestAnswerCreateModel> ExamAndTestAnswerCreateModels { get; set; }
    }
    public class ExamAndTestQuestionUpdateModel
    {
        [MaxLength(255)]
        public string? ExamAndTestQuestionContent { get; set; }
        public int? Tier { get; set; }
        [MaxLength(100)]
        public string? KindOfQuestion { get; set; }
        [MaxLength(50)]
        public string? ExamAndTestQuestionType { get; set; }
        public Guid? SubjectId { get; set; }
        public Guid? FacultyId { get; set; }
    }
}
