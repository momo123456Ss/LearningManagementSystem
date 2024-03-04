using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.MailSender;

namespace LearningManagementSystem.Repository.InterfaceRepository
{
    public interface InterfaceMailRepository
    {
        Task<APIResponse> SendMail(MailData mailData);
    }
}
