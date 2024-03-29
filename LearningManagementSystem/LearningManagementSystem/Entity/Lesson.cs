﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LearningManagementSystem.Entity
{
    [Table("Lesson")]
    public class Lesson
    {
        public Lesson()
        {
            LessonResourcess = new HashSet<LessonResources>();
            QuestionAndAnswers = new HashSet<QuestionAndAnswer>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LessonId { get; set; }
        [MaxLength(100)]
        public string? LessonTitle { get; set; }
        //Khóa ngoại
        //many-to-one
        #region
        public int? SubjectTopicId { get; set; }
        [ForeignKey("SubjectTopicId")]
        public SubjectTopic SubjectTopicNavigation { get; set; }
        #endregion
        //one-to-many
        #region
        public ICollection<LessonResources> LessonResourcess { get; set; }
        [JsonIgnore]
        public ICollection<QuestionAndAnswer> QuestionAndAnswers { get; set; }

        #endregion
    }
}
