using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LearningManagementSystem.Entity
{
    [Table("LecturesAndResources")]
    public class LecturesAndResources
    {
        public LecturesAndResources()
        {
            LessonResourcess = new HashSet<LessonResources>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(25)]
        public string? TypeOfDocument { get; set; }
        [MaxLength(25)]
        public string? Status { get; set; }
        public DateTime? UploadDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool? Approve { get; set; }
        [MaxLength(100)]
        public string? Note { get; set; }

        //Thông tin File
        [MaxLength(25)]
        public string? FileType { get; set; }
        [MaxLength(100)]
        public string? FileName { get; set; }
        [MaxLength(25)]
        public string? FileSize { get; set; }
        [MaxLength(255)]
        public string? FileUrl { get; set; }
        [MaxLength(255)]
        public string? FileViewUrl { get; set; }
       

        //Khóa ngoại
        //many-to-one
        public Guid SubjectId {  get; set; }
        [ForeignKey("SubjectId")]
        public Subject SubjectNavigation {  get; set; }

        //one-to-many
        #region
        [JsonIgnore]
        public ICollection<LessonResources> LessonResourcess { get; set; }
        #endregion
    }
}
