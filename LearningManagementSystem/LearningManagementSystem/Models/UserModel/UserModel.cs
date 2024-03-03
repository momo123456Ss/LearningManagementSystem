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
    //ViewModel
    public class UserViewModel
    {
        public string UserCode { get; set; }
        public string FullName {  get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public UserRoleViewModel UserRoleViewModel { get; set; }

    }
}
