using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.FacultyModel;
using LearningManagementSystem.Models.SubjectModel;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly InterfaceSubjectRepository _interfaceSubjectRepository;
        public SubjectController(InterfaceSubjectRepository interfaceSubjectRepository)
        {
            this._interfaceSubjectRepository = interfaceSubjectRepository;
        }
        //GET
        #region
        [HttpGet("GetAllSubject")]
        public async Task<IActionResult> GetAllSubject(string? searchString, int page = 1)
        {
            try
            {
                return Ok(await _interfaceSubjectRepository.GetAll(searchString, page));
            }
            catch (Exception ex)
            {
                return BadRequest(
                   new APIResponse
                   {
                       Success = false,
                       Message = $"Error GetAllSubject: {ex.Message}"
                   });
            }
        }
        [HttpGet("GetSubjectById/{id}")]
        public async Task<IActionResult> GetSubjectById(String id)
        {
            try
            {
                return Ok(await _interfaceSubjectRepository.GetById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error GetSubjectById: {ex.Message}"
                });
            }
        }
        [HttpGet("GetSubjectByTeacherId")]
        [Authorize]
        public async Task<IActionResult> GetSubjectByUserId(string? searchString, string? sortBy, int page = 1)
        {
            try
            {
                return Ok(await _interfaceSubjectRepository.GetSubjectByUserId(searchString,sortBy,page));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error GetSubjectByTeacherId: {ex.Message}"
                });
            }
        }
        #endregion
        //POST
        #region
        [HttpPost("CreateSubject")]
        public async Task<IActionResult> CreateSubject([FromBody] SubjectModelCreate model)
        {
            try
            {
                return Ok(await _interfaceSubjectRepository.CreateNew(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error CreateSubject: {ex.Message}"
                });
            }
        }
        #endregion
        //PUT
        #region
        [HttpPut("UpdateSubject/{id}")]
        public async Task<IActionResult> UpdateSubject(string id, [FromBody] SubjectModelUpdate model)
        {
            try
            {
                return Ok(await _interfaceSubjectRepository.UpdateById(id, model));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error UpdateSubject: {ex.Message}"
                });
            }
        }
        [HttpPut("UpdateLastRecentBySubjectId/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateLastRecentBySubjectId(string id)
        {
            try
            {
                return Ok(await _interfaceSubjectRepository.UpdateLastRecentBySubjectId(id));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error UpdateSubject: {ex.Message}"
                });
            }
        }
        #endregion
    }
}
