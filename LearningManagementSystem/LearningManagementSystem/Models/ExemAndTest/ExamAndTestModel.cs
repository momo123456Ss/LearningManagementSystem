using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Models.ExemAndTest
{
    public class ExamAndTestModelUploadFile
    {
        [MaxLength(100)]
        public string? FileName { get; set; }
        [MaxLength(50)]
        public string? Time { get; set; }
        [Required]
        public Guid SubjectId { get; set; }
        [Required]
        public Guid FacultyId { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
    public class ExamAndTestModelDowload
    {
        public string? FileUrl { get; set; }
        public string? FileName { get; set; }
    }
}
