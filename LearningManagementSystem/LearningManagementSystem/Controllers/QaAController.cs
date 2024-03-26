using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.QaAModel;
using LearningManagementSystem.Models.SubjectModel;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QaAController : ControllerBase
    {
        private readonly InterfaceQaARepository _InterfaceQaARepository;
        public QaAController(InterfaceQaARepository interfaceQaARepository)
        {
            this._InterfaceQaARepository = interfaceQaARepository;
        }
        //GET
        #region
        [HttpGet("GetQaA/{subjectId}")]
        [Authorize]
        public async Task<IActionResult> GetQaA(string subjectId, string? classId, int? subjectTopicId, int? lessonId, string? sortByCreatedDate, bool QaAIsFollow,bool myQuestions, bool QuestionsNoAnswer, int page = 1)
        {
            try
            {
                return Ok(await _InterfaceQaARepository.GetQaA(subjectId,classId, subjectTopicId,lessonId,sortByCreatedDate, QaAIsFollow,myQuestions, QuestionsNoAnswer,page));
            }
            catch (Exception ex)
            {
                return BadRequest(
                   new APIResponse
                   {
                       Success = false,
                       Message = $"Error GetQaA: {ex.Message}"
                   });
            }
        }
        #endregion
        //POST
        #region
        [HttpPost("CreateQaA")]
        [Authorize]
        public async Task<IActionResult> CreateQaA([FromBody] QaAModelCreate model)
        {
            try
            {
                return Ok(await _InterfaceQaARepository.CreateQaA(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error CreateQaA: {ex.Message}"
                });
            }
        }
        [HttpPost("QaAFollow/{QaAId}")]
        [Authorize]
        public async Task<IActionResult> QaAFollow(int QaAId)
        {
            try
            {
                return Ok(await _InterfaceQaARepository.QaAFollow(QaAId));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error QaAFollow: {ex.Message}"
                });
            }
        }
        #endregion
        //PUT
        #region
        [HttpPut("QaALike/{QaAId}")]
        [Authorize]
        public async Task<IActionResult> QaALike(int QaAId)
        {
            try
            {
                return Ok(await _InterfaceQaARepository.QaALike(QaAId));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error QaAId: {ex.Message}"
                });
            }
        }
        #endregion
    }
}
