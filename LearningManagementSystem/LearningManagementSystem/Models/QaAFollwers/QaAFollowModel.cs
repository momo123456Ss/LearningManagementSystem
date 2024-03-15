using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.QaAModel;
using LearningManagementSystem.Models.UserModel;

namespace LearningManagementSystem.Models.QaAFollwers
{
    public class QaAFollowViewModel
    {
        public int QaAFollowersId { get; set; }
        public UserLiteViewModel UserIdFollowerNavigation { get; set; }
        public QaALiteViewModel QuestionAndAnswerNavigation { get; set; }
    }
}
