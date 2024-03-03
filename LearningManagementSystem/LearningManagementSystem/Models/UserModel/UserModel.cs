using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LearningManagementSystem.Models.UserModel
{
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
}
