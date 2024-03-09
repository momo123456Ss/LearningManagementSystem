using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningManagementSystem.Entity
{
    [Table("LessonResources")]
    public class LessonResources
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //Khóa ngoại
        //many-to-one
        #region
        public int? LecturesAndResourcesId { get; set; }
        public int? LessonId { get; set; }
        public Guid? ClassId { get; set; }
        //--
        [ForeignKey("LecturesAndResourcesId")]
        public LecturesAndResources LecturesAndResourcesNavigation { get; set; }

        [ForeignKey("LessonId")]
        public Lesson LessonNavigation { get; set; }

        [ForeignKey("ClassId")]
        public Class ClassNavigation { get; set; }
        #endregion
    }
}
