using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.ClassModel;
using LearningManagementSystem.Models.LessonModel;
using LearningManagementSystem.Models.UserModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Models.QaAModel
{
    public class QaAModelCreate
    {
        [Required]
        [MaxLength(50)]
        public string QuestionAndAnswerTitle { get; set; }
        [Required]
        [MaxLength(255)]
        public string QuestionAndAnswerContent { get; set; }
        [Required]
        public Guid ClassIdComment { get; set; }
        [Required]
        public int LessonIdComment { get; set; }
        public int? QaAInOtherQaA { get; set; }
        public int? QaAReplyQaA { get; set; }
    }
    public class QaAModelView
    {
        public string QuestionAndAnswerTitle { get; set; }
        public string QuestionAndAnswerContent { get; set; }
        public Guid ClassIdComment { get; set; }
        public int LessonIdComment { get; set; }
    }
    public class QaAModelUserView
    {
        public int QuestionAndAnswerId { get; set; }
        public string QuestionAndAnswerTitle { get; set; }
        public string QuestionAndAnswerContent { get; set; }
        public int countLike { get; set; }
        public int NumberOfResponses { get; set; }
        public DateTime? QaACreatedDate { get; set; }
        public int? QaAInOtherQaA { get; set; }
        public int? QaAReplyQaA { get; set; }
        public UserViewModel UserViewModelNavigation { get; set; }
        public ClassModelView ClassModelViewNavigation { get; set; }
        public LessonModelView LessonModelViewNavigation { get; set; }
    }
    public class QaALiteViewModel
    {
        public int QuestionAndAnswerId { get; set; }
        public string QuestionAndAnswerTitle { get; set; }
        public string QuestionAndAnswerContent { get; set; }
        public ClassModelView ClassModelViewNavigation { get; set; }
        public LessonLiteViewModel LessonModelViewNavigation { get; set; }
    }
}
