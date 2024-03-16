using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningManagementSystem.Entity
{
    [Table("ExamAndTest")]
    public class ExamAndTest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        //many-to-one
        public Guid SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public Subject SubjectNavigation { get; set; }
        public Guid FacultyId { get; set; }
        [ForeignKey("FacultyId")]
        public Faculty FacultyNavigation { get; set; }
    }
}
