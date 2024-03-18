using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.ExamAndTestQuestionModel;
using LearningManagementSystem.Models.QaAModel;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EaTQuestionController : ControllerBase
    {
        private readonly InterfaceEaTQuestionRepository _interfaceEaTQuestionRepository;
        public EaTQuestionController(InterfaceEaTQuestionRepository interfaceEaTQuestionRepository)
        {
            _interfaceEaTQuestionRepository = interfaceEaTQuestionRepository;
        }
        //GET
        #region
        #endregion
        //POST
        #region
        [HttpPost("CreateMultipeQuestion")]
        [Authorize (Policy = "RequireTeacher")]
        public async Task<IActionResult> CreateMultipeQuestion([FromBody] List<ExamAndTestQuestionCreateModel> models)
        {
            try
            {
                return Ok(await _interfaceEaTQuestionRepository.CreateQuestions(models));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error CreateMultipeQuestion: {ex.Message}"
                });
            }
        }
        [HttpPost("CreateQuestion")]
        [Authorize(Policy = "RequireTeacher")]
        public async Task<IActionResult> CreateQuestion([FromBody] ExamAndTestQuestionCreateModel model)
        {
            try
            {
                return Ok(await _interfaceEaTQuestionRepository.CreateQuestion(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error CreateQuestion: {ex.Message}"
                });
            }
        }
        #endregion
        //PUT
        #region
        [HttpPut("UpdateQuestion/{questionId}")]
        [Authorize]
        public async Task<IActionResult> UpdateQuestion(string questionId,[FromBody] ExamAndTestQuestionUpdateModel model)
        {
            try
            {
                return Ok(await _interfaceEaTQuestionRepository.UpdateQuestion(questionId,model));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error CreateQuestion: {ex.Message}"
                });
            }
        }
        #endregion
        //DELETE
        #region
        [HttpDelete("DeleteQuestion/{questionId}")]
        [Authorize]
        public async Task<IActionResult> DeleteQuestion(string questionId)
        {
            try
            {
                return Ok(await _interfaceEaTQuestionRepository.DeleteQuestion(questionId));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error DeleteQuestion: {ex.Message}"
                });
            }
        }
        #endregion
    }
}
