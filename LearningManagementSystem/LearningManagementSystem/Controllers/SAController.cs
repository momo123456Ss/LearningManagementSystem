using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.MailSender;
using LearningManagementSystem.Models.SAModel;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SAController : ControllerBase
    {
        private readonly InterfaceSARepository _interfaceSARepository;
        public SAController(InterfaceSARepository interfaceSARepository)
        {
            this._interfaceSARepository = interfaceSARepository;
        }
        //GET
        #region
        #endregion
        //POST
        #region
        [HttpPost("CreateSA")]
        [Authorize(Policy = "RequireTeacher")]
        public async Task<IActionResult> CreateSA([FromBody] SubjectAnnouncementModelCreate model)
        {
            try
            {
                return Ok(await _interfaceSARepository.CreateSA(model));
            }catch (Exception ex)
            {
                return BadRequest(
                    new APIResponse
                    {
                        Success = false,
                        Message = $"Error : {ex.Message}"
                    });
            }
        }
        #endregion
        //PUT
        #region
        #endregion
    }
}
