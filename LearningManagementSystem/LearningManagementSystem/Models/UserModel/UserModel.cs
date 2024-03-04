using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LearningManagementSystem.Models.UserRoleModels;

namespace LearningManagementSystem.Models.UserModel
{
    //Model
    public class UserModelUpdateAllType
    {
        //Thông tin cá nhân
        #region
        [MaxLength(100)]
        public string? FirstName { get; set; }
        [MaxLength(50)]
        public string? LastName { get; set; }
        [MaxLength(25)]
        public string? Phone { get; set; }
        [MaxLength(25)]
        public string? Gender { get; set; }
        [MaxLength(100)]
        public string? Address { get; set; }
        #endregion
    }
    public class UserModelAllType
    {
        [Required]
        public Guid? RoleId { get; set; }
        public IFormFile? imageFile { get; set; }

        //Thông tin SignIn
        #region
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [MaxLength(255)]
        public string Password { get; set; }
        [DefaultValue(false)]
        public bool IsActived { get; set; }
        #endregion

        //Thông tin cá nhân
        #region
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(25)]
        public string Phone { get; set; }
        [Required]
        [MaxLength(25)]
        public string Gender { get; set; }
        [Required]
        [MaxLength(100)]
        public string Address { get; set; }
        #endregion
    }

    public class UserModelChangeAvatar
    {
        public IFormFile? imageFile { get; set; }
    }
    public class UserModelUpdateRole
    {
        public Guid? RoleId { get; set; }
    }
    public class UserChangePasswordModel
    {
        [Required]
        [MaxLength(255)]
        public string CurrentPassword { get; set; }
        [Required]
        [MaxLength(255)]
        public string NewPassword { get; set; }
    }
    public class LeadershipModelUpdateNotificationSettings
    {
        //Thông báo Tài khoản người dùng
        #region
        public bool? NotificationWhenUpdatingAccount { get; set; }
        public bool? NotificationWhenChangingPassword { get; set; }
        #endregion
        //Cài đặt thông báo của Leadership
        #region
        //Thông báo phân quyền
        public bool? LeadershipNotificationWhenYouMakeChangesInTheRoleList { get; set; }
        public bool? LeadershipNotificationWhenYouMakeChangesInTheUserList { get; set; }
        //Thông báo ngân hàng đề thi
        public bool? LeadershipNotificationWhenInstructorsSaveNewExamQuestionsIntoTheSystem { get; set; }
        public bool? LeadershipNotificationWhenYouConfirmOrCancelTheTest { get; set; }
        //Thông báo tệp riêng tư
        public bool? LeadershipNotificationWhenYouCreateOrChangeNamesOrDeletePrivateFiles { get; set; }
        //Thông báo quản lý môn học
        public bool? LeadershipNotificationWhenThereAreChangesInSubjectContent { get; set; }
        public bool? LeadershipNotificationWhenThereAreChangesInSubjectManagement { get; set; }
        #endregion
    }
    public class TeacherModelUpdateNotificationSettings
    {
        //Thông báo Tài khoản người dùng
        #region
        public bool? NotificationWhenUpdatingAccount { get; set; }
        public bool? NotificationWhenChangingPassword { get; set; }
        #endregion
        //Cài đặt thông báo của Teacher
        #region
        //Thông báo của Đề thi & Kiểm tra
        public bool? TeacherNotificationWhenYouUploadOrCreateNewTestQuestionsAndRenameTestQuestions { get; set; }
        public bool? TeacherNotificationWhenYouUpdateTheTestBankWhenUploadOrCreateNewOrEditAndDelete { get; set; }
        //Thông báo bài giảng và tài nguyên
        public bool? TeacherNotificationWhenYouCreateOrChangeTheNameOrDeleteALectureAndMoveTheLectureToTheSubjectTopic { get; set; }
        public bool? TeacherNotifyWhenYouCreateOrChangeTheNameOrDeleteAResourcesAndMoveTheResourcesToTheSubjectTopic { get; set; }
        //Thông báo môn giảng dạy
        public bool? TeacherNotificationWhenYouAddDocumentsOrUpdateDocumentsAndAssignDocumentsToTeachingClasses { get; set; }
        public bool? TeacherNotificationWhenSomeoneAsksAQuestionInTheCourseOrInteractsWithYourAnswer { get; set; }
        public bool? TeacherNotificationWhenSomeoneCommentsOnTheCourseAnnouncement { get; set; }
        #endregion
    }
    public class StudentModelUpdateNotificationSettings
    {
        #region
        //Thông báo Tài khoản người dùng
        public bool? NotificationWhenUpdatingAccount { get; set; }
        public bool? NotificationWhenChangingPassword { get; set; }
        #endregion
        //Cài đặt thông báo của Student
        #region
        //Thông báo môn học
        public bool? StudentNotificationsWhenInstructorsCreateSubjectAnnouncements { get; set; }
        public bool? StudentNotificationsWhenSomeoneCommentsOnASubjectAnnouncement { get; set; }
        //Thông báo hỏi đáp
        public bool? StudentNotificationsWhenTheLecturerAsksAQuestionInTheSubject { get; set; }
        public bool? StudentNotificationsWhenSomeoneInteractsWithYourQuestionOrAnswer { get; set; }
        #endregion
    }
    //ViewModel
    public class UserViewModel
    {
        public string UserCode { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public UserRoleViewModel UserRoleViewModel { get; set; }

    }
}
