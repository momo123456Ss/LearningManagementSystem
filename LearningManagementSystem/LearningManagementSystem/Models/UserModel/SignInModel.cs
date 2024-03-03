using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Models.UserModel
{
    public class SignInModel
    {
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        [Required]
        [MaxLength(250)]
        public string Password { get; set; }
    }
}
