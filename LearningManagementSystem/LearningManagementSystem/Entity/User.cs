using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

namespace LearningManagementSystem.Entity
{
    [Table("User")]
    [Index(nameof(User.Email), IsUnique = true)]
    [Index(nameof(User.UserCode), IsUnique = true)]
    public class User
    {
        public User()
        {
            RefreshTokens = new HashSet<RefreshToken>();
        }
        [Key]
        public Guid UserId {  get; set; }
        [Required]
        [MaxLength(20)]
        public string UserType { get; set; }
        [Required]
        [MaxLength(100)]
        public string UserCode { get; set; }
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
        [MaxLength(255)]
        public string Avatar { get; set; }
        [Required]
        [MaxLength(100)]
        public string FirstName {  get; set; }
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


        //Cài đặt thông báo của Leadership
        #region
        //Thông báo phân quyền
        [DefaultValue(false)]
        public bool LeadershipNotificationWhenYouMakeChangesInTheRoleList { get; set; }
        [DefaultValue(false)]
        public bool LeadershipNotificationWhenYouMakeChangesInTheUserList { get; set; }
        //Thông báo ngân hàng đề thi
        [DefaultValue(false)]
        public bool LeadershipNotificationWhenInstructorsSaveNewExamQuestionsIntoTheSystem { get; set; }
        [DefaultValue(false)]
        public bool LeadershipNotificationWhenYouConfirmOrCancelTheTest {  get; set; }
        //Thông báo tệp riêng tư
        [DefaultValue(false)]
        public bool LeadershipNotificationWhenYouCreateOrChangeNamesOrDeletePrivateFiles { get; set; }
        //Thông báo quản lý môn học
        [DefaultValue(false)]
        public bool LeadershipNotificationWhenThereAreChangesInSubjectContent {  get; set; }
        [DefaultValue(false)]
        public bool LeadershipNotificationWhenThereAreChangesInSubjectManagement { get; set; }
        #endregion
        //Cài đặt thông báo của Teacher
        #region
        //Thông báo của Đề thi & Kiểm tra
        [DefaultValue(false)]
        public bool TeacherNotificationWhenYouUploadOrCreateNewTestQuestionsAndRenameTestQuestions {  get; set; }
        [DefaultValue(false)]
        public bool TeacherNotificationWhenYouUpdateTheTestBankWhenUploadOrCreateNewOrEditAndDelete {  get; set; }
        //Thông báo bài giảng và tài nguyên
        [DefaultValue(false)]
        public bool TeacherNotificationWhenYouCreateOrChangeTheNameOrDeleteALectureAndMoveTheLectureToTheSubjectTopic { get; set; }
        [DefaultValue(false)]
        public bool TeacherNotifyWhenYouCreateOrChangeTheNameOrDeleteAResourcesAndMoveTheResourcesToTheSubjectTopic { get;set; }
        //Thông báo môn giảng dạy
        [DefaultValue(false)]
        public bool TeacherNotificationWhenYouAddDocumentsOrUpdateDocumentsAndAssignDocumentsToTeachingClasses {  get; set; }
        [DefaultValue(false)]
        public bool TeacherNotificationWhenSomeoneAsksAQuestionInTheCourseOrInteractsWithYourAnswer {  get; set; }
        [DefaultValue(false)]
        public bool TeacherNotificationWhenSomeoneCommentsOnTheCourseAnnouncement { get; set; }
        #endregion
        //Cài đặt thông báo của Student
        #region
        //Thông báo môn học
        [DefaultValue (false)]
        public bool StudentNotificationsWhenInstructorsCreateSubjectAnnouncements { get; set; }
        [DefaultValue(false)]
        public bool StudentNotificationsWhenSomeoneCommentsOnASubjectAnnouncement { get; set; }
        //Thông báo hỏi đáp
        [DefaultValue(false)]
        public bool StudentNotificationsWhenTheLecturerAsksAQuestionInTheSubject { get; set; }
        [DefaultValue(false)]
        public bool StudentNotificationsWhenSomeoneInteractsWithYourQuestionOrAnswer { get; set; }
        #endregion
        //Cài đặt thông báo dùng chung
        #region
        //Thông báo Tài khoản người dùng
        [DefaultValue(false)]
        public bool NotificationWhenUpdatingAccount { get; set; }
        [DefaultValue(false)]
        public bool NotificationWhenChangingPassword { get; set; }
        #endregion



        //Khóa ngoại
        //Many-to-one
        #region
        public Guid RoleId { get; set; }
        [ForeignKey("RoleId")]
        public UserRole UserRole { get; set; }
        #endregion
        //One-to-many
        #region
        [JsonIgnore]
        public ICollection<RefreshToken> RefreshTokens { get; set; }
        #endregion

    }
}
