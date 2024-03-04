using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.MailSender;
using LearningManagementSystem.Repository.InterfaceRepository;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Security.Claims;

namespace LearningManagementSystem.Repository
{
    public class MailRepository : InterfaceMailRepository
    {
        private readonly MailSettings _mailSettings;
        private readonly LearningManagementSystemContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MailRepository(IOptions<MailSettings> mailSettingsOptions, LearningManagementSystemContext context
            , IHttpContextAccessor httpContextAccessor)
        {
            this._mailSettings = mailSettingsOptions.Value;
            this._context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<APIResponse> SendMail(MailData mailData)
        {
            var user = await _context.Users.Include(role => role.UserRole)
                           .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
            
            using (MimeMessage emailMessage = new MimeMessage())
            {                
                MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                emailMessage.From.Add(emailFrom);
                MailboxAddress emailTo = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                emailMessage.To.Add(emailTo);
                //thêm mail CC và BCC nếu cần
                //emailMessage.Cc.Add(new MailboxAddress("Cc Receiver", "cc@example.com"));
                //emailMessage.Bcc.Add(new MailboxAddress("Bcc Receiver", "bcc@example.com"));
                emailMessage.Subject = "Feedback from users of Learning Management System";/*mailData.Title;*/
                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody =
                    "User feedback information:"
                    + "<br/>UserId: " + user.UserId.ToString()
                    + "<br/>Role: " + user.UserRole.RoleName
                    + "<br/>User code: " + user.UserCode
                    + "<br/>Email: " + user.Email 
                    + "<br/>Full name: " + user.FirstName + " " + user.LastName 
                    + "<br/>Response content:"
                    + "<br/><b>" + mailData.Body + "</b>";
                emailMessage.Body = emailBodyBuilder.ToMessageBody();
                using (SmtpClient mailClient = new SmtpClient())
                {
                    mailClient.Connect(_mailSettings.SmtpServer, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                    mailClient.Authenticate(_mailSettings.SenderEmail, _mailSettings.Password);
                    mailClient.Send(emailMessage);
                    mailClient.Disconnect(true);
                }
            }
            return new APIResponse { Success = true, Message = "Send feedback success." };
        }
    }
}
