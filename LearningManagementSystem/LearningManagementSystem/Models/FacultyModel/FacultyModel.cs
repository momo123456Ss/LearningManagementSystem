using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LearningManagementSystem.Models.FacultyModel
{
    public class FacultyModelCreate
    {
        [Required]
        [MaxLength(50)]
        public string FacultyCode { get; set; }
        [Required]
        [MaxLength(100)]
        public string FacultyName { get; set; }
        [Required]
        [MaxLength(255)]
        public string Description { get; set; }
        [Required]
        [MaxLength(255)]
        public string ContactInformation { get; set; }
        [Required]
        [MaxLength(255)]
        public string Address { get; set; }
    }
    public class FacultyModelUpdate
    {
        [MaxLength(255)]
        public string? Description { get; set; }
        [MaxLength(255)]
        public string? ContactInformation { get; set; }
        [MaxLength(255)]
        public string? Address { get; set; }
    }
    public class FacultyModelView
    {
        public Guid? FacultyId { get; set; }
        public string? FacultyCode { get; set; }
        public string? FacultyName { get; set; }
        public string? Description { get; set; }
        public string? ContactInformation { get; set; }
        public string? Address { get; set; }
        public DateTime? EstablishmentDate { get; set; }
        public int? NumberOfStudents { get; set; }
        public int? NumberOfTeacher { get; set; }
    }
}
