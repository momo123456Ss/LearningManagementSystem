using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.ClassModel;
using LearningManagementSystem.Models.SubjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Models.UserClassSubjectModel
{
    public class UserClassSubjectModelCreate
    {
        [Required]
        public Guid? ClassId { get; set; }
        [Required]
        public Guid? SubjectId { get; set; }
        public List<Guid> UserId { get; set; }       
    }
    public class UserClassSubjectModelView
    {
        public ClassModelView? ClassNavigation { get; set; }
        public SubjectModelView? SubjectNavigation { get; set; }
        public DateTime? LastRecent { get; set; }
        public bool? Mark { get; set; }
    }
}
