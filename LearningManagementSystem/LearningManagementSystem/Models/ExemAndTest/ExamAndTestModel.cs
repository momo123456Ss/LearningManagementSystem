using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.FacultyModel;
using LearningManagementSystem.Models.SubjectModel;
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
    public class ExamAndTestViewModel
    {
        public int ExamAndTestId { get; set; }
        [MaxLength(50)]
        public string? ExamForm { get; set; }
        [MaxLength(50)]
        public string? Time { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool? Approve { get; set; }
        [MaxLength(100)]
        public string? Note { get; set; }
        public string? Status { get; set; }

        //Thông tin File
        [MaxLength(25)]
        public string? FileType { get; set; }
        [MaxLength(100)]
        public string? FileName { get; set; }
        [MaxLength(25)]
        public string? FileSize { get; set; }
        [MaxLength(255)]
        public string? FileUrl { get; set; }
        [MaxLength(255)]
        public string? FileViewUrl { get; set; }
        //Khóa ngoại
        public SubjectLiteViewModel SubjectNavigation { get; set; }
        public FacultyLiteViewModel FacultyNavigation { get; set; }

    }
}
