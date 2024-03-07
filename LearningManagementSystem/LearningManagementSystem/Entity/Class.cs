using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningManagementSystem.Entity
{
    [Table("Class")]
    public class Class
    {
        [Key]
        public Guid ClassId { get; set; }
        [MaxLength(50)]
        public string ClassCode { get; set; }
        [Required]
        [MaxLength(100)]
        public string ClassName { get; set; }
        [Required]
        [MaxLength(100)]
        public string AcademicYear { get; set; }
        [MaxLength(10)]
        public string? Semester {  get; set; }
        [Required]
        public DateTime ClassOpeningDay { get; set; }
        public DateTime ClassClosingDay { get; set;}
        //khóa ngoại
        //many-to-one
        #region
        public Guid? Faculty { get; set; }
        [ForeignKey("Faculty")]
        public Faculty FacultyNavigation { get; set; }
        #endregion
    }
}
