using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.ClassModel;
using LearningManagementSystem.Models.SubjectModel;
using LearningManagementSystem.Models.UserModel;
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
    public class SubjectAnnouncementModelCreateSingle
    {
        [Required]
        public Guid ClassIdAnnouncement { get; set; }
        [Required]
        public Guid SubjectIdAnnouncement { get; set; }
        [MaxLength(50)]
        public string? SubjectAnnouncementTitle { get; set; }
        [Required]
        [MaxLength]
        public string SubjectAnnouncementContent { get; set; }
        public int? SAInOtherSA { get; set; }
        public int? SAReplySA { get; set; }
    }
    public class SubjectAnnouncementModelView
    {
        public int SubjectAnnouncementId { get; set; }
        public string? SubjectAnnouncementTitle { get; set; }
        public string SubjectAnnouncementContent { get; set; }
        public int? SAInOtherSA { get; set; }
        public int? SAReplySA { get; set; }
        public UserViewModel UserModelViewNavigation { get; set; }
        public ClassModelView ClassModelViewNavigation { get; set; }
        public DateTime SACreatedDate { get; set; }
    }
    public class SubjectAnnouncementLiteViewModel
    {
        public int SubjectAnnouncementId { get; set; }
        public string? SubjectAnnouncementTitle { get; set; }
        public string SubjectAnnouncementContent { get; set; }
        public DateTime SACreatedDate { get; set; }
        public SubjectLiteViewModel? SubjectAnnouncementNavigation { get; set; }
        public ClassModelView ClassModelViewNavigation { get; set; }
    }

}
