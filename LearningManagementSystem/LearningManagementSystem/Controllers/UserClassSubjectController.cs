using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.UserBelongToFacultyModel;
using LearningManagementSystem.Models.UserClassSubjectModel;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserClassSubjectController : ControllerBase
    {
        private readonly InterfaceUserClassSubjectRepository _interfaceUserClassSubjectRepository;
        public UserClassSubjectController(InterfaceUserClassSubjectRepository interfaceUserClassSubjectRepository)
        {
            this._interfaceUserClassSubjectRepository = interfaceUserClassSubjectRepository;
        }
        //GET
        #region
        [HttpGet("GetSubjectStudent-AcademicYear/{academicYear}/Semester/{semester}")]
        [Authorize(Policy = "RequireStudent")]
        public async Task<IActionResult> GetSubjectStudent(string academicYear, string semester, string? searchString, string? sortBy, bool? mark, int page = 1)
        {
            try
            {
                return Ok(await _interfaceUserClassSubjectRepository.GetSubjectStudent(academicYear,semester,searchString,sortBy,mark,page));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error GetSubjectStudent: {ex.Message}"
                });
            }
        }
        #endregion
        //POST
        #region
        [HttpPost("CreatorUserClassSubject")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> CreatorUserClassSubject([FromBody] UserClassSubjectModelCreate model)
        {
            try
            {
                return Ok(await _interfaceUserClassSubjectRepository.CreatorNew(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error CreatorUserClassSubject: {ex.Message}"
                });
            }
        }
        #endregion
        //PUT
        #region
        [HttpPut("UpdateLastRecentBySubjectId/{subjetcId}/And/{classId}")]
        [Authorize(Policy = "RequireStudent")]
        public async Task<IActionResult> UpdateLastRecent(string subjetcId, string classId)
        {
            try
            {
                return Ok(await _interfaceUserClassSubjectRepository.UpdateLastRecent(subjetcId,classId));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error UpdateLastRecent: {ex.Message}"
                });
            }
        }
        [HttpPut("UpdateMarkBySubjectId/{subjetcId}/And/{classId}")]
        [Authorize(Policy = "RequireStudent")]
        public async Task<IActionResult> UpdateMark(string subjetcId, string classId)
        {
            try
            {
                return Ok(await _interfaceUserClassSubjectRepository.UpdateMark(subjetcId, classId));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error UpdateMark: {ex.Message}"
                });
            }
        }
        #endregion
    }
}
