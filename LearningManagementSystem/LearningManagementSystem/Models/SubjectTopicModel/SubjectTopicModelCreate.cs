﻿using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.LessonModel;
using LearningManagementSystem.Models.LessonResources;
using LearningManagementSystem.Models.SubjectModel;
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
        public ICollection<LessonModelView>? LessonNavigation { get; set; }

    }
    public class SubjectTopicLiteViewModel
    {
        public int? Id { get; set; }
        public string? SubjectTopicTitle { get; set; }
        public SubjectLiteViewModel SubjectIdNavigation { get; set; }
    }
}
