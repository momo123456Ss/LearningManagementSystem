using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.QaAFollwers;
using LearningManagementSystem.Models.QaAModel;
using LearningManagementSystem.Models.SAModel;
using LearningManagementSystem.Models.UserModel;
using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Models.UserNotificationsModel
{
    public class UserNotificationModelCreate
    {
        [MaxLength(100)]
        public string? UserNotificationsContent { get; set; }
        [Required]
        public Guid UserIdNotifications { get; set; }
        public int? QaAFollowersId { get; set; }// Thông báo theo dõi bình luận
        public int? SubjectAnnouncementId { get; set; }// Thông báo giảng viên đăng thông báo mới
        public int? QuestionAndAnswerId { get; set; }// Thông báo giảng viên vừa thêm câu hỏi trong bài giảng - chủ đề - môn học
    }
    public class UserNotificationViewModel
    {
        public int UserNotificationsId { get; set; }
        public string? UserNotificationsContent { get; set; }
        public DateTime? CreatedDate { get; set; }
        public UserLiteViewModel? UserLiteViewModel { get; set; }//Thông báo của người dùng nào
        public QaAFollowViewModel? QaAFollowersNavigation { get; set; }//Theo ai đã dõi bình luận
        public SubjectAnnouncementLiteViewModel? SubjectAnnouncementNavigation { get; set; }// Thông báo giảng viên đăng thông báo mới của môn học
        public QaALiteViewModel? QuestionAndAnswerNavigation { get; set; }// Thông báo giảng viên vừa
                                                                          // thêm câu hỏi trong bài giảng - chủ đề - môn học
                                                                          // hoặc là đã trả lời bình luận 



    }
}
