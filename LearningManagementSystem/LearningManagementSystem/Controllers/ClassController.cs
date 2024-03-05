using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.ClassModel;
using LearningManagementSystem.Models.UserRoleModels;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly InterfaceClassRepository _interfaceClassRepository;
        public ClassController(InterfaceClassRepository interfaceClassRepository)
        {
            this._interfaceClassRepository = interfaceClassRepository;
        }
        //GET
        #region
        [HttpGet("GetAllClassOpenDateStartBetween/{startString}/And/{endString}")]
        public async Task<IActionResult> GetAllClassOpenDateStartBetween(string startString, string endString)
        {
            try
            {
                return Ok(await _interfaceClassRepository.GetAllClassOpenDateStartBetween(startString, endString));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error GetAllClassOpenDateStartBetween: {ex.Message}"
                });
            }
        }
        [HttpGet("GetAllClassHasNotYetEnded")]
        public async Task<IActionResult> GetAllClassHasNotYetEnded()
        {
            try
            {
                return Ok(await _interfaceClassRepository.GetAllClassHasNotYetEnded());
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error GetAllClassHasNotYetEnded: {ex.Message}"
                });
            }
        }
        [HttpGet("GetAllClassHasEnded")]
        public async Task<IActionResult> GetAllClassHasEnded()
        {
            try
            {
                return Ok(await _interfaceClassRepository.GetAllClassHasEnded());
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error GetAllClassHasEnded: {ex.Message}"
                });
            }
        }
        [HttpGet("GetAllClassDoesNotHaveAnEndDateYet")]
        public async Task<IActionResult> GetAllClassDoesNotHaveAnEndDateYet()
        {
            try
            {
                return Ok(await _interfaceClassRepository.GetAllClassDoesNotHaveAnEndDateYet());
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error GetAllClassDoesNotHaveAnEndDateYet: {ex.Message}"
                });
            }
        }
        [HttpGet("GetAllClass")]
        public async Task<IActionResult> GetAllClass()
        {
            try
            {
                return Ok(await _interfaceClassRepository.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error GetAllClass: {ex.Message}"
                });
            }
        }
        [HttpGet("GetClassById/{id}")]
        public async Task<IActionResult> GetClassById(String id)
        {
            try
            {
                return Ok(await _interfaceClassRepository.GetById(id));
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
        [HttpPost("CreateNewClass")]
        public async Task<IActionResult> CreateNewClass([FromBody] ClassModelCreate model)
        {
            try
            {
                return Ok(await _interfaceClassRepository.CreateNewClass(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error CreateNewClass: {ex.Message}"
                });
            }
        }
        #endregion
        //PUT
        #region
        [HttpPut("UpdateClass/{id}")]
        public async Task<IActionResult> UpdateClass(string id, [FromBody] ClassModelUpdate model)
        {
            try
            {
                return Ok(await _interfaceClassRepository.UpdateById(id, model));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error UpdateClass: {ex.Message}"
                });
            }
        }
        #endregion
    }
}
