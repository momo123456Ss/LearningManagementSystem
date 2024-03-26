using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.UserBelongToFacultyModel;
using LearningManagementSystem.Models.UserRoleModels;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBelongToFacultyController : ControllerBase
    {
        private readonly InterfaceUserBelongToFacultyRepository _interfaceUserBelongToFacultyRepository;
        public UserBelongToFacultyController(InterfaceUserBelongToFacultyRepository interfaceUserBelongToFacultyRepository)
        {
            _interfaceUserBelongToFacultyRepository = interfaceUserBelongToFacultyRepository;
        }
        //GET
        #region
        
        #endregion
        //POST
        #region
        [HttpPost("CreatorUserBelongToFaculty")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> CreatorUserBelongToFaculty([FromBody] UserBelongToFacultyModelCreate model)
        {
            try
            {
                return Ok(await _interfaceUserBelongToFacultyRepository.CreatorUserBelongToFaculty(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error CreatorUserBelongToFaculty: {ex.Message}"
                });
            }
        }
        #endregion
        //PUT
        #region
        [HttpPut("SetHeadOfDepartment/{userId}/faculty/{facultyId}")]
        [Authorize (Policy = "RequireAdministrator")]
        public async Task<IActionResult> SetHeadOfDepartment(string userId, string facultyId)
        {
            try
            {
                var response = await _interfaceUserBelongToFacultyRepository.SetHeadOfDepartmentByUserIdAndFacultyId(userId, facultyId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error setting head of department: {ex.Message}" });
            }
        }
        #endregion
    }
}
