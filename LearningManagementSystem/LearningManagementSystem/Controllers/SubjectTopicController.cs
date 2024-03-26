using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.OtherSubjectInformationModel;
using LearningManagementSystem.Models.SubjectTopicModel;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectTopicController : ControllerBase
    {
        private readonly InterfaceSubjectTopicRepository _interfaceSubjectTopicRepository;
        public SubjectTopicController(InterfaceSubjectTopicRepository interfaceSubjectTopicRepository)
        {
            this._interfaceSubjectTopicRepository = interfaceSubjectTopicRepository;
        }
        //GET
        #region
        //[HttpGet("GetById/{subjectTopicId}/class/{classId}")]
        //public async Task<IActionResult> GetById(string subjectTopicId, string classId)
        //{
        //    try
        //    {
        //        return Ok(await _interfaceSubjectTopicRepository.GetById(subjectTopicId, classId));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new APIResponse
        //        {
        //            Success = false,
        //            Message = $"Error GetById: {ex.Message}"
        //        });
        //    }
        //}
        #endregion
        //POST
        #region
        [HttpPost("SubjectTopicCreate")]
        [Authorize (Policy = "RequireAdministratorAndTeacher")]
        public async Task<IActionResult> SubjectTopicCreate([FromBody] SubjectTopicModelCreate model)
        {
            try
            {
                return Ok(await _interfaceSubjectTopicRepository.CreateNew(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error SubjectTopicCreate: {ex.Message}"
                });
            }
        }
        #endregion
        //PUT
        #region
        [HttpPut("UpdateSubjectTopic/{id}")]
        [Authorize(Policy = "RequireAdministratorAndTeacher")]
        public async Task<IActionResult> UpdateSubjectTopic(string id, [FromBody] SubjectTopicModelUpdate model)
        {
            try
            {
                return Ok(await _interfaceSubjectTopicRepository.UpdateById(id, model));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error UpdateSubjectTopic: {ex.Message}"
                });
            }
        }
        #endregion
    }
}
