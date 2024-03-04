using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Models.UserModel
{
    public class SignInModel
    {
        [Required]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        [DefaultValue("nino02022002@gmail.com")]
        public string Email { get; set; }
        [Required]
        [MaxLength(250)]
        [DataType(DataType.Password)]
        [DefaultValue("123456")]
        public string Password { get; set; }
    }
}
