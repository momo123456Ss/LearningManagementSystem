using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.ExamAndTestQuestionModel;
using LearningManagementSystem.Models.ExamAndTestQuestionModel.AnswerModel;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EaTAnswerController : ControllerBase
    {
        private readonly InterfaceEaTAnswerRepository _interfaceEaTAnswerRepository;
        public EaTAnswerController(InterfaceEaTAnswerRepository interfaceEaTAnswerRepository)
        {
            _interfaceEaTAnswerRepository = interfaceEaTAnswerRepository;
        }
        //POST
        #region
        [HttpPost("CreateAnswer")]
        [Authorize(Policy = "RequireTeacher")]
        public async Task<IActionResult> CreateAnswer([FromBody] ExamAndTestAnswerAddOrUpdateModel model)
        {
            try
            {
                return Ok(await _interfaceEaTAnswerRepository.AddAnswer(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error CreateAnswer: {ex.Message}"
                });
            }
        }
        #endregion
        //PUT
        #region
        [HttpPut("UpdateAnswer/{answerId}")]
        [Authorize(Policy = "RequireTeacher")]
        public async Task<IActionResult> UpdateAnswer(string answerId, [FromBody] ExamAndTestAnswerAddOrUpdateModel model)
        {
            try
            {
                return Ok(await _interfaceEaTAnswerRepository.UpdateAnswer(answerId, model));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error UpdateAnswer: {ex.Message}"
                });
            }
        }
        #endregion
        //DELETE
        #region
        [HttpDelete("DeleteAnswer/{answerId}")]
        [Authorize]
        public async Task<IActionResult> DeleteAnswer(string answerId)
        {
            try
            {
                return Ok(await _interfaceEaTAnswerRepository.DeleteAnswer(answerId));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error DeleteAnswer: {ex.Message}"
                });
            }
        }
        #endregion
    }
}
