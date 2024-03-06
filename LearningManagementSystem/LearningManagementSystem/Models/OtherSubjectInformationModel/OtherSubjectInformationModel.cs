using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Models.OtherSubjectInformationModel
{
    public class OtherSubjectInformationModelCreate
    {
        [Required]
        [MaxLength(50)]
        public string OtherSubjectTitle { get; set; }
        [Required]
        [MaxLength(255)]
        public string OtherSubjectDescription { get; set; }
        [Required]
        public Guid SubjectId { get; set; }
    }
    public class OtherSubjectInformationModelUpdate
    {
        public string? OtherSubjectTitle { get; set; }
        public string? OtherSubjectDescription { get; set; }
    }
    public class OtherSubjectInformationModelView
    {
        public int Id { get; set; }
        public string? OtherSubjectTitle { get; set; }
        public string? OtherSubjectDescription { get; set; }
    }
}
