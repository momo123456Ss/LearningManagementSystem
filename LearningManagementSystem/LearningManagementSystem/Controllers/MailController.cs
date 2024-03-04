using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.MailSender;
using LearningManagementSystem.Repository.InterfaceRepository;
using MailKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly InterfaceMailRepository _interfaceMailRepository;
        public MailController(InterfaceMailRepository interfaceMailRepository)
        {
            this._interfaceMailRepository = interfaceMailRepository;
        }
        [HttpPost("SendFeedback")]
        [Authorize]
        public async Task<IActionResult> SendMail(MailData mailData)
        {
            try
            {
                return Ok(await _interfaceMailRepository.SendMail(mailData));

            }catch (Exception ex)
            {
                return BadRequest(
                    new APIResponse
                    {
                        Success = false,
                        Message = $"Error SendMail: {ex.Message}"
                    });
            }
        }
    }
}
