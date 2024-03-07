using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningManagementSystem.Entity
{
    [Table("UserClassSubject")]
    public class UserClassSubject
    {
        public Guid? UserId { get; set; }
        public Guid? ClassId { get; set; }
        public Guid? SubjectId { get; set; }
        public DateTime? LastRecent {  get; set; }
        [DefaultValue(false)]
        public bool Mark {  get; set; }
        //Khóa ngoại
        //many-to-one
        #region
        [ForeignKey("UserId")]
        public User UserNavigation { get; set; }
        [ForeignKey("ClassId")]
        public Class ClassNavigation { get; set; }
        [ForeignKey("SubjectId")]
        public Subject SubjectNavigation { get; set; }
        #endregion
    }
}
