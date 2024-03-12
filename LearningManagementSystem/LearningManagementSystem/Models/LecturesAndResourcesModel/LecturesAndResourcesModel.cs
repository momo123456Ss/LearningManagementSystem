using LearningManagementSystem.Models.SubjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Models.LecturesAndResourcesModel
{
    public class LecturesAndResourcesModelCreate
    {
        [Required]
        public Guid SubjectId { get; set; }
        public List<IFormFile> Files { get; set; }
    }
    public class LecturesAndResourcesModelView
    {
        public int? Id { get; set; }
        public string? TypeOfDocument { get; set; }
        public string? Status { get; set; }
        public DateTime? UploadDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool? Approve { get; set; }
        public string? Note { get; set; }
        //Thông tin File
        public string? FileType { get; set; }
        public string? FileName { get; set; }
        public string? FileSize { get; set; }
        public string? FileUrl { get; set; }
        public string? FileViewUrl { get; set; }
        public Subject_LecturesAndResourcesModelView? SubjectNavigation { get; set; }
    }
    public class LecturesAndResourcesModelLessonView
    {
        public int? Id { get; set; }
        public string? TypeOfDocument { get; set; }
        public string? FileType { get; set; }
        public string? FileName { get; set; }
        public string? FileSize { get; set; }
        public string? FileUrl { get; set; }
        public string? FileViewUrl { get; set; }
        public bool? Approve { get; set; }
    }
    public class LecturesAndResourcesModelDowload
    {
        public string? FileUrl { get; set; }
        public string? FileName { get; set; }
    }
    public class LecturesAndResourcesModelChageFile
    {
        public IFormFile? newFile { get; set; }
    }
}
