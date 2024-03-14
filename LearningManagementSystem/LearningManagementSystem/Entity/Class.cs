using LearningManagementSystem.Migrations;
using Org.BouncyCastle.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LearningManagementSystem.Entity
{
    [Table("Class")]
    public class Class
    {
        public Class()
        {
            UserClassSubjects = new HashSet<UserClassSubject>();
            //LessonResourcess = new HashSet<LessonResources>();
            QuestionAndAnswers = new HashSet<QuestionAndAnswer>();
            SubjectAnnouncements = new HashSet<SubjectAnnouncement>();
        }
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
        //one-to-many
        #region
        [JsonIgnore]
        public ICollection<UserClassSubject> UserClassSubjects { get; set; }
        [JsonIgnore]
        public ICollection<QuestionAndAnswer> QuestionAndAnswers { get; set; }
        //[JsonIgnore]
        //public ICollection<LessonResources> LessonResourcess { get; set; }
        [JsonIgnore]
        public ICollection<SubjectAnnouncement> SubjectAnnouncements { get; set; }
        #endregion
    }
}
