using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.ClassModel;
using LearningManagementSystem.Models.SubjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningManagementSystem.Models.LearningStatisticsModel
{
    public class LearningStatisticsModelCreate
    {
        [Required]
        public Guid ClassId { get; set; }
        [Required]
        public Guid SubjectId { get; set; }
    }
    public class LearningStatisticsViewModel
    {
        public ClassLiteViewModel ClassNavigation { get; set; }
        public SubjectLiteViewModel SubjectNavigation { get; set; }
        public double? totalMinutes { get; set; }

    }
}
