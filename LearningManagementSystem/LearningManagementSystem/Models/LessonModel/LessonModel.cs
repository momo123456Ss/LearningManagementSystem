using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.LessonResources;
using LearningManagementSystem.Models.SubjectTopicModel;
using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Models.LessonModel
{
    public class LessonModelView
    {
        public int LessonId { get; set; }
        public string? LessonTitle { get; set; }
        //public ICollection<LessonResourcesView> LessonResourcess { get; set; }

    }
    public class LessonLiteViewModel
    {
        public int LessonId { get; set; }
        public string? LessonTitle { get; set; }
        public SubjectTopicLiteViewModel SubjectTopicNavigation { get; set; }
    }
    public class LessonModelView2
    {
        public int LessonId { get; set; }
        public string? LessonTitle { get; set; }
        public ICollection<LessonResourcesView> LessonResourcess { get; set; }

    }
    public class LessonModelCreate
    {
        [Required]
        public int SubjectTopicId { get; set; }
        [Required]
        public string LessonTitle { get; set; }
        public ICollection<int> LectureId { get; set; }
        public ICollection<int> ResourceId { get; set; }
        public ICollection<Guid> ClassId { get; set; }
    }
}
