using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LearningManagementSystem.Entity
{
    [Table("SubjectAnnouncement")]
    public class SubjectAnnouncement
    {
        public SubjectAnnouncement()
        {
            SAInOtherSAs = new HashSet<SubjectAnnouncement>();
            SAReplySAs = new HashSet<SubjectAnnouncement>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubjectAnnouncementId { get; set; }
        [MaxLength(50)]
        public string? SubjectAnnouncementTitle { get; set;}
        [Required]
        [MaxLength]
        public string SubjectAnnouncementContent { get; set; }
        public DateTime SACreatedDate { get; set; }

        //Khóa ngoại vòng về SA
        public int? SAInOtherSA { get; set; } // Để biết bình luận này năm trong bình luận nào (Phần trả lời)
        [ForeignKey("SAInOtherSA")]
        public SubjectAnnouncement? SAInOtherSANavigation { get; set; }
        public int? SAReplySA { get; set; }// Để biết bình luận này trả lởi bình luận nào (Phần trả lời)
        [ForeignKey("SAReplySA")]
        public SubjectAnnouncement? SAReplySANavigation { get; set; }
        //Khóa ngoại
        //Many-to-one
        #region
        [Required]
        public Guid UserIdAnnouncement { get; set; }
        [ForeignKey("UserIdAnnouncement")]
        public User UserAnnouncementNavigation { get; set; }
        [Required]
        public Guid ClassIdAnnouncement { get; set; }
        [ForeignKey("ClassIdAnnouncement")]
        public Class ClassAnnouncementNavigation { get; set; }
        [Required]
        public Guid SubjectIdAnnouncement { get; set; }
        [ForeignKey("SubjectIdAnnouncement")]
        public Subject SubjectAnnouncementNavigation { get; set; }
        #endregion
        //one-to-many
        #region
        [JsonIgnore]
        public ICollection<SubjectAnnouncement> SAInOtherSAs { get; set; }
        [JsonIgnore]
        public ICollection<SubjectAnnouncement> SAReplySAs { get; set; }
        #endregion

    }
}
