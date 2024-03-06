using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Models.SubjectTopicModel
{
    public class SubjectTopicModelCreate
    {
        [Required]
        public Guid? SubjectId { get; set; }
        [Required]
        [MaxLength(100)]
        public string? SubjectTopicTitle { get; set; }
    }
    public class SubjectTopicModelUpdate
    {
        public string? SubjectTopicTitle { get; set; }
    }
    public class SubjectTopicModelView
    {
        public int? Id { get; set; }
        public string? SubjectTopicTitle { get; set; }

    }
}
