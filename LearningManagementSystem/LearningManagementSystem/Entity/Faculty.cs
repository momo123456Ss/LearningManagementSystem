using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LearningManagementSystem.Entity
{
    [Table("Faculty")]
    public class Faculty
    {
        public Faculty()
        {
            UserBelongToFacultys = new HashSet<UserBelongToFaculty>();
            Classes = new HashSet<Class>(); 
            ExamAndTests = new HashSet<ExamAndTest>();
            ExamAndTestQuestionss = new HashSet<ExamAndTestQuestions>();
        }
        //Thông tin Faculty
        [Key]
        public Guid FacultyId { get; set; }
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
        public DateTime EstablishmentDate { get; set; }
        [MaxLength(255)]
        public string ContactInformation {get; set; }
        [MaxLength(255)]
        public string Address { get; set; }
        [DefaultValue(0)]
        public int NumberOfStudents {  get; set; }
        [DefaultValue(0)]
        public int NumberOfTeacher { get; set; }
        //Khóa ngoại
        //Many-to-one
        #region
        #endregion
        //One-to-many
        #region
        [JsonIgnore]
        public ICollection<UserBelongToFaculty> UserBelongToFacultys { get; set; }
        [JsonIgnore]
        public ICollection<Class> Classes { get; set; }
        [JsonIgnore]
        public ICollection<ExamAndTest> ExamAndTests { get; set; }
        [JsonIgnore]
        public ICollection<ExamAndTestQuestions> ExamAndTestQuestionss { get; set; }
        #endregion
    }
}
