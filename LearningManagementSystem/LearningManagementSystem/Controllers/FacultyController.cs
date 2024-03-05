using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.ClassModel;
using LearningManagementSystem.Models.FacultyModel;
using LearningManagementSystem.Models.UserRoleModels;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultyController : ControllerBase
    {
        private readonly InterfaceFacultyRepository _interfaceFacultyRepository;

        public FacultyController(InterfaceFacultyRepository interfaceFacultyRepository) {
            this._interfaceFacultyRepository = interfaceFacultyRepository;
        }
        //GET
        #region
        [HttpGet("GetAllFaculty")]
        public async Task<IActionResult> GetAllFaculty(string? searchString, string? sortBy, int page = 1)
        {
            try
            {
                return Ok(await _interfaceFacultyRepository.GetAll(searchString, sortBy, page));
            }
            catch (Exception ex)
            {
                return BadRequest(
                   new APIResponse
                   {
                       Success = false,
                       Message = $"Error GetAllFaculty: {ex.Message}"
                   });
            }
        }
        [HttpGet("GetFacultyById/{id}")]
        public async Task<IActionResult> GetFacultyById(String id)
        {
            try
            {
                return Ok(await _interfaceFacultyRepository.GetById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error GetClassById: {ex.Message}"
                });
            }
        }
        #endregion
        //POST
        #region
        [HttpPost("CreateFaculty")]
        public async Task<IActionResult> CreateFaculty([FromBody] FacultyModelCreate model)
        {
            try
            {
                return Ok(await _interfaceFacultyRepository.CreateNew(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error CreateFaculty: {ex.Message}"
                });
            }
        }
        #endregion
        //PUT
        #region
        [HttpPut("UpdateFaculty/{id}")]
        public async Task<IActionResult> UpdateFaculty(string id, [FromBody] FacultyModelUpdate model)
        {
            try
            {
                return Ok(await _interfaceFacultyRepository.UpdateById(id, model));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error UpdateFaculty: {ex.Message}"
                });
            }
        }
        #endregion
    }
}
