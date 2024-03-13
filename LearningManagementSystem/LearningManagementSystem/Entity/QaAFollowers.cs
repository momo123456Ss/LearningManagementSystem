using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningManagementSystem.Entity
{
    [Table("QaAFollowers")]
    public class QaAFollowers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QaAFollowersId { get; set; }

        //Khóa
        public Guid UserIdFollower { get; set; }
        public int QaAIdFollow{ get; set; }
        [ForeignKey("UserIdFollower")]
        public User UserIdFollowerNavigation { get; set; }
        [ForeignKey("QaAIdFollow")]
        public QuestionAndAnswer QuestionAndAnswerNavigation { get; set; }
    }
}
