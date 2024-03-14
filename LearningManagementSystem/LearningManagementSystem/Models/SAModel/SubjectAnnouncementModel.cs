using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Models.SAModel
{
    public class SubjectAnnouncementModelCreate
    {
        [Required]
        public ICollection<Guid> ListClassId { get; set; }
        [Required]
        public Guid SubjectIdAnnouncement { get; set; }
        [Required]
        [MaxLength(50)]
        public string SubjectAnnouncementTitle { get; set; }
        [Required]
        [MaxLength]
        public string SubjectAnnouncementContent { get; set; }
        //public int? SAInOtherSA { get; set; }
        //public int? SAReplySA { get; set; }
    }
}
