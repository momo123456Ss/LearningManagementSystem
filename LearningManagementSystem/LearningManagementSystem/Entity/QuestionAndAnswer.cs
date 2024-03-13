using LearningManagementSystem.Migrations;
using Org.BouncyCastle.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LearningManagementSystem.Entity
{
    [Table("QuestionAndAnswer")]
    public class QuestionAndAnswer
    {
        public QuestionAndAnswer()
        {
            QaAInOtherQaAs = new HashSet<QuestionAndAnswer>();
            QaAReplyQaAs = new HashSet<QuestionAndAnswer>();
            QaAFollowerss = new HashSet<QaAFollowers>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionAndAnswerId { get; set; }
        [Required]
        [MaxLength(50)]
        public string QuestionAndAnswerTitle { get; set; }
        [Required]
        [MaxLength(255)]
        public string QuestionAndAnswerContent { get; set; }
        [DefaultValue(0)]
        public int countLike { get; set; }
        [DefaultValue(0)]
        public int NumberOfResponses { get; set; }
        public DateTime? QaACreatedDate { get; set; }
        //Khóa ngoại vòng về Q&A
        #region
        public int? QaAInOtherQaA { get; set; } // Để biết bình luận này năm trong bình luận nào (Phần trả lời)
        [ForeignKey("QaAInOtherQaA")]
        public QuestionAndAnswer? QaAInOtherQaANavigation { get; set; }
        public int? QaAReplyQaA { get; set; }// Để biết bình luận này trả lởi bình luận nào (Phần trả lời)
        [ForeignKey("QaAReplyQaA")]
        public QuestionAndAnswer? QaAReplyQaANavigation { get; set; }
        #endregion

        //Khóa ngoại
        //Many-to-one
        #region
        [Required]
        public Guid UserIdComment {  get; set; }
        [ForeignKey("UserIdComment")]
        public User UserNavigation { get; set; }
        [Required]
        public Guid ClassIdComment { get; set; }
        [ForeignKey("ClassIdComment")]
        public Class ClassNavigation { get; set; }
        [Required]
        public int LessonIdComment { get; set; }
        [ForeignKey("LessonIdComment")]
        public Lesson LessonNavigation { get; set; }
        #endregion

        //one-to-many
        #region
        [JsonIgnore]
        public ICollection<QuestionAndAnswer> QaAInOtherQaAs { get; set; }
        [JsonIgnore]
        public ICollection<QuestionAndAnswer> QaAReplyQaAs { get; set; }
        [JsonIgnore]
        public ICollection<QaAFollowers> QaAFollowerss { get; set; }
        #endregion
    }
}
