using System.ComponentModel;

namespace LearningManagementSystem.Models.MailSender
{
    public class MailData
    {
        //[DisplayName("Địa chỉ email người nhận")]
        //[DefaultValue("2051010290thien@ou.edu.vn")]
        //public string ReceiverEmail { get; set; }
        //[DisplayName("Tên người nhận")]
        //[DefaultValue("ThienHuynh")]
        //public string ReceiverName { get; set; }
        //[DisplayName("Tiêu đề")]
        //[DefaultValue("Hello")]
        //public string Title { get; set; }
        [DisplayName("Nội dung")]
        [DefaultValue("Tôi muốn phản hồi về tài liệu tiếng Anh của mình chưa được phê duyệt")]
        public string Body { get; set; }
    }
}
