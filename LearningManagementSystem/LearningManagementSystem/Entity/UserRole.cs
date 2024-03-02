using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LearningManagementSystem.Entity
{
    [Table("UserRole")]
    [Index(nameof(UserRole.RoleName), IsUnique = true)]
    public class UserRole
    {
        public UserRole()
        {
            Users = new HashSet<User>();
        }
        [Key]
        public Guid RoleId { get; set; }
        [Required]
        [MaxLength(50)]
        public string? RoleName { get; set; }
        [Required]
        [MaxLength(255)]
        public string? Describe{ get; set; }

        //Liên kết đến Bảng (One-to-many)
        #region
        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }
        #endregion


        //Phân quyền Môn học
        #region
        [DefaultValue(false)]
        public bool SubjectSee {  get; set; }
        [DefaultValue(false)]
        public bool SubjectEdit { get; set; }
        #endregion
        //Phân quyền Tệp riêng tư
        #region
        [DefaultValue(false)]
        public bool PrivateFilesSee { get; set; }
        [DefaultValue(false)]
        public bool PrivateFilesEdit { get; set; }
        [DefaultValue(false)]
        public bool PrivateFilesDelete { get; set; }
        [DefaultValue(false)]
        public bool PrivateFilesCreateNew { get; set; }
        [DefaultValue(false)]
        public bool PrivateFilesDownload { get; set; }
        #endregion
        //Phân quyền Bài giảng và Tài nguyên
        #region
        [DefaultValue(false)]
        public bool LecturesAndResourcesSee { get; set; }
        [DefaultValue(false)]
        public bool LecturesAndResourcesEdit { get; set; }
        [DefaultValue(false)]
        public bool LecturesAndResourcesDelete { get; set; }
        [DefaultValue(false)]
        public bool LecturesAndResourcesCreateNew { get; set; }
        [DefaultValue(false)]
        public bool LecturesAndResourcesDownload { get; set; }
        [DefaultValue(false)]
        public bool LecturesAndResourcesAddToSubject { get; set; }
        #endregion
        //Phân quyền Đề thi và kiểm tra
        #region
        [DefaultValue(false)]
        public bool ExamsAndTestsSee { get; set; }
        [DefaultValue(false)]
        public bool ExamsAndTestsEdit { get; set; }
        [DefaultValue(false)]
        public bool ExamsAndTestsDelete { get; set; }
        [DefaultValue(false)]
        public bool ExamsAndTestsCreateNew { get; set; }
        [DefaultValue(false)]
        public bool ExamsAndTestsDownload { get; set; }
        [DefaultValue(false)]
        public bool ExamsAndTestsAcceptance { get; set; }
        #endregion
        //Phân quyền thông báo 
        #region
        [DefaultValue(false)]
        public bool NotificationSee { get; set; }
        [DefaultValue(false)]
        public bool NotificationEdit { get; set; }
        [DefaultValue(false)]
        public bool NotificationDelete { get; set; }
        [DefaultValue(false)]
        public bool NotificationSettings {get; set; }
        #endregion
        //Phân quyền 
        #region
        [DefaultValue(false)]
        public bool DecentralizationSee { get; set; }
        [DefaultValue(false)]
        public bool DecentralizationEdit { get; set; }
        [DefaultValue(false)]
        public bool DecentralizationDelete { get; set; }
        [DefaultValue(false)]
        public bool DecentralizationCreateNew { get; set; }
        #endregion
        //Phân quyền tài khoản người dùng
        #region
        [DefaultValue(false)]
        public bool UserAccountSee { get; set; }
        [DefaultValue(false)]
        public bool UserAccountEdit { get; set; }
        #endregion
    }
}
