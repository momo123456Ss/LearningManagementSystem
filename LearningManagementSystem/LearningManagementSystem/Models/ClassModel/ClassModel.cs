using LearningManagementSystem.Models.FacultyModel;
using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Models.ClassModel
{
    public class ClassLiteViewModel
    {
        public Guid ClassId { get; set; }
        public string? ClassCode { get; set; }
        public string? ClassName { get; set; }
        public string? Semester { get; set; }
        public string? AcademicYear { get; set; }     
        public FacultyLiteViewModel? FacultyNavigation { get; set; }
    }
    public class ClassModelView
    {
        public Guid ClassId { get; set; }
        public string? ClassCode { get; set; }
        public string? ClassName { get; set; }
        public string? Semester { get; set; }
        public string? AcademicYear { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? ClassOpeningDay { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? ClassClosingDay { get; set; }
        public FacultyModelView? FacultyModelViewNavigation { get; set; }
    }
    public class ClassModelCreate
    {
        [Required]
        public Guid? Faculty { get; set; }
        [MaxLength(50)]
        public string ClassCode { get; set; }
        [Required]
        [MaxLength(100)]
        public string ClassName { get; set; }
        [Required]
        [MaxLength(100)]
        public string AcademicYear { get; set; }
        [Required]
        [MaxLength(10)]
        public string? Semester { get; set; }
        [Required]
        public DateTime ClassOpeningDay { get; set; }
        public DateTime? ClassClosingDay { get; set; }
    }
    public class ClassModelUpdate
    {
        public Guid? Faculty { get; set; }
        [MaxLength(50)]
        public string? ClassCode { get; set; }
        [MaxLength(100)]
        public string? ClassName { get; set; }
        [MaxLength(1)]
        public string? Semester { get; set; }
        [MaxLength(100)]
        public string? AcademicYear { get; set; }
        public DateTime? ClassOpeningDay { get; set; }
        public DateTime? ClassClosingDay { get; set; }
    }
}
