using LearningManagementSystem.Models.LessonResources;

namespace LearningManagementSystem.Models.LessonModel
{
    public class LessonModelView
    {
        public int LessonId { get; set; }
        public string? LessonTitle { get; set; }
        public ICollection<LessonResourcesView> LessonResourcess { get; set; }

    }
    //public class LessonModelView
    //{
    //    public int LessonId { get; set; }
    //    public string? LessonTitle { get; set; }
    //}
}
