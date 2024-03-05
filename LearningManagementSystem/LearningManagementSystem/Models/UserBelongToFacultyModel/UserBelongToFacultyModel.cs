using LearningManagementSystem.Entity;
using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Models.UserBelongToFacultyModel
{
    public class UserBelongToFacultyModelCreate
    {
        [Required]
        public Guid FacultyId { get; set; }
        public List<Guid> UserId { get; set; }
    }
}
