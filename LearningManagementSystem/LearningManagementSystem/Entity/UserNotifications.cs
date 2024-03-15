using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningManagementSystem.Entity
{
    [Table("UserNotifications")]
    public class UserNotifications
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserNotificationsId { get; set; }
        [MaxLength]
        public string? UserNotificationsContent { get; set; }
        [Required]
        public Guid UserIdNotifications { get; set; }//Thông báo của người dùng
        [ForeignKey("UserIdNotifications")]
        public User UserNotificationsNavigation { get; set; }
        public DateTime? CreatedDate { get; set; }

        //Loại thông báo
        public int? QaAFollowersId { get; set; }// Thông báo theo dõi bình luận
        [ForeignKey("QaAFollowersId")]
        public QaAFollowers? QaAFollowersNavigation { get; set; }
        public int? SubjectAnnouncementId { get; set; }// Thông báo giảng viên đăng thông báo mới
        [ForeignKey("SubjectAnnouncementId")]
        public SubjectAnnouncement? SubjectAnnouncementNavigation { get; set; }
        public int? QuestionAndAnswerId { get; set; }// Thông báo giảng viên vừa thêm câu hỏi trong bài giảng - chủ đề - môn học
        [ForeignKey("QuestionAndAnswerId")]
        public QuestionAndAnswer? QuestionAndAnswerNavigation { get; set; }
    }
}
