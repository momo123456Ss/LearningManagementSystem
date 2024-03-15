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
        [HttpGet("GetSA/{subjectId}")]
        [Authorize]
        public async Task<IActionResult> GetSA(string subjectId, string? classId, bool isNotice, int page = 1)
        {
            try
            {
                return Ok(await _interfaceSARepository.GetSA(subjectId,classId,isNotice,page));
            }
            catch (Exception ex)
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
        [HttpPost("CreateSASingle")]
        [Authorize]
        public async Task<IActionResult> CreateSA([FromBody] SubjectAnnouncementModelCreateSingle model)
        {
            try
            {
                return Ok(await _interfaceSARepository.CreateSA(model));
            }
            catch (Exception ex)
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
