using LearningManagementSystem.Migrations;
using Org.BouncyCastle.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Printing;
using System.Text.Json.Serialization;

namespace LearningManagementSystem.Entity
{
    [Table("Subject")]
    public class Subject
    {
        public Subject()
        {
            OtherSubjectInformations = new HashSet<OtherSubjectInformation>();
            SubjectTopics = new HashSet<SubjectTopic>();
            UserClassSubjects = new HashSet<UserClassSubject>();
            LecturesAndResourcesL = new HashSet<LecturesAndResources>();
        }
        [Key]
        public Guid SubjectId {get; set;}
        [Required]
        [MaxLength(50)]
        public string SubjectCode { get; set; }
        [Required]
        [MaxLength(50)]
        public string SubjectName { get; set;}
        [Required]
        [MaxLength(255)]
        public string SubjectDescription { get; set; }

        public DateTime? LastRecent {  get; set; }
        //Khóa ngoại
        //Many-to-one
        #region
        public Guid? LecturerId { get; set; }
        [ForeignKey("LecturerId")]
        public User UserNavigation { get; set; }
        #endregion
        //one-to-many
        #region
        public ICollection<OtherSubjectInformation> OtherSubjectInformations { get; set; }
        public ICollection<SubjectTopic> SubjectTopics { get; set; }
        [JsonIgnore]
        public ICollection<UserClassSubject> UserClassSubjects { get; set; }
        [JsonIgnore]
        public ICollection<LecturesAndResources> LecturesAndResourcesL { get; set; }
        #endregion
    }
}
