using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.OtherSubjectInformationModel;
using LearningManagementSystem.Models.SubjectTopicModel;
using LearningManagementSystem.Models.UserModel;
using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Models.SubjectModel
{
    public class SubjectModelCreate
    {
        [Required]
        [MaxLength(50)]
        public string SubjectCode { get; set; }
        [Required]
        [MaxLength(50)]
        public string SubjectName { get; set; }
        [Required]
        [MaxLength(255)]
        public string SubjectDescription { get; set; }
        [Required]
        public Guid? LecturerId { get; set; }

    }
    public class SubjectModelUpdate
    {
        [MaxLength(50)]
        public string? SubjectCode { get; set; }
        [MaxLength(50)]
        public string? SubjectName { get; set; }
        [MaxLength(255)]
        public string? SubjectDescription { get; set; }
        public Guid? LecturerId { get; set; }

    }
    public class SubjectModelView
    {
        public Guid SubjectId { get; set; }
        public string? SubjectCode { get; set; }
        public string? SubjectName { get; set; }
        public string? SubjectDescription { get; set; }
        public UserViewModel? Lecturer { get; set; }
        public DateTime? LastRecent { get; set; }
        public ICollection<OtherSubjectInformationModelView>? OtherSubjectInformationModelViews { get; set; }
        public ICollection<SubjectTopicModelView>? SubjectTopics { get; set; }

    }
    public class Subject_LecturesAndResourcesModelView
    {
        public Guid SubjectId { get; set; }
        public string? SubjectCode { get; set; }
        public string? SubjectName { get; set; }
        public string? SubjectDescription { get; set; }
        public UserViewModel? Lecturer { get; set; }
    }
    public class SubjectLiteViewModel
    {
        public Guid SubjectId { get; set; }
        public string? SubjectCode { get; set; }
        public string? SubjectName { get; set; }
    }
}
