using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.LecturesAndResourcesModel;
using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Models.LessonResources
{
    public class LessonLectureModelCreate
    {
        [Required]
        public string LessonTitle { get; set; }
        [Required]
        public int SubjectTopicId { get; set; }
        [Required]
        public int LecturesAndResourcesId { get; set; }
        public ICollection<Guid>? ClassId { get; set; }
        
    }
    public class LessonResourcesModelCreate
    {
        [Required]
        public int LessonId { get; set; }
        public ICollection<int>? LecturesAndResourcesId { get; set; }

        public ICollection<Guid>? ClassId { get; set; }

    }
    public class LessonResourcesView
    {
        public Guid? ClassId { get; set; }
        public LecturesAndResourcesModelLessonView? LecturesAndResourcesNavigation { get; set; }
    }
    public class FileIdAndTopicTitle
    {
        public string? TopicTitle { get; set; }
        public List<int>? FileId { get; set; }
    }
}
