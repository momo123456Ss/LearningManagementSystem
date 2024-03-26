using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.ExamAndTestQuestionModel;
using LearningManagementSystem.Models.ExamAndTestQuestionModel.AnswerModel;
using LearningManagementSystem.Models.QaAModel;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

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
        [HttpGet("ExamExcelGenerate")]
        [Authorize (Policy = "RequireTeacher")]
        public async Task<IActionResult> GenerateExamExcel(string tendethi, string facultyId, string subjectId, string hinhthuc, string thoigianthi, int easy, int normal, int difficult)
        {
            try
            {
                return Ok(await _interfaceEaTQuestionRepository.GenerateExamExcel(tendethi, facultyId, subjectId, hinhthuc, thoigianthi, easy, normal, difficult));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("ExamWordGenerate")]
        [Authorize(Policy = "RequireTeacher")]
        public async Task<IActionResult> GenerateExamDocument(string tendethi,string facultyId, string subjectId, string made, string hinhthuc, string thoigianthi, int easy, int normal, int difficult)
        {
            
            byte[] documentBytes = await _interfaceEaTQuestionRepository.GenerateExamDocument(facultyId, subjectId, made,hinhthuc,thoigianthi,easy,normal,difficult);

            // Trả về tệp Word
            return File(documentBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"{tendethi}-{DateTime.Now}.docx");
        }
        [HttpGet("GetQuestion/{questionId}")]
        [Authorize(Policy = "RequireTeacher")]
        public async Task<IActionResult> GetQuestion(string questionId)
        {
            try
            {
                return Ok(await _interfaceEaTQuestionRepository.GetById(questionId));
            }
            catch (Exception ex)
            {
                return BadRequest(
                   new APIResponse
                   {
                       Success = false,
                       Message = $"Error GetQuestion: {ex.Message}"
                   });
            }
        }
        [HttpGet("GetAllQuestion")]
        [Authorize(Policy = "RequireTeacher")]
        public async Task<IActionResult> GetAllQuestion(string? searchString, string? facultyId, string? subjectId, int? tier, int page = 1)
        {
            try
            {
                return Ok(await _interfaceEaTQuestionRepository.GetAll(searchString, facultyId, subjectId, tier, page));
            }
            catch (Exception ex)
            {
                return BadRequest(
                   new APIResponse
                   {
                       Success = false,
                       Message = $"Error GetAllQuestion: {ex.Message}"
                   });
            }
        }
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
        [Authorize(Policy = "RequireTeacher")]
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
        [Authorize(Policy = "RequireTeacher")]
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
