using LearningManagementSystem.Models.UserRoleModels;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly InterfaceUserRoleRepository _InterfaceUserRoleRepository;

        public UserRoleController(InterfaceUserRoleRepository InterfaceUserRoleRepository) {
            this._InterfaceUserRoleRepository = InterfaceUserRoleRepository;
        }
        [HttpPost("CreateNewRole")]
        public async Task<IActionResult> CreateNewRole([FromBody] UserRoleModel model)
        {
            try
            {
                return Ok(await _InterfaceUserRoleRepository.CreateNew(model));
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(String id)
        {
            try
            {
                return Ok(await _InterfaceUserRoleRepository.GetById(id));
            }
            catch
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }
        [HttpGet("GetAllUserRole")]
        public async Task<IActionResult> GetAllUserRole()
        {
            try
            {
                return Ok(await _InterfaceUserRoleRepository.GetAll());
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpDelete("DeleteUserRole/{id}")]
        public async Task<IActionResult> GetAllDeleteUserRoleUserRole(string id)
        {
            try
            {
                return Ok(await _InterfaceUserRoleRepository.DeleteById(id));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPut("UpdateUserRole/{id}")]
        public async Task<IActionResult> UpdateUserRole(string id, [FromBody] UserRoleModel model)
        {
            try
            {
                return Ok(await _InterfaceUserRoleRepository.UpdateById(id, model));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
