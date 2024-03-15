using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UNController : ControllerBase
    {
        private readonly InterfaceUNRepository _interfaceUNRepository;
        public UNController(InterfaceUNRepository interfaceUNRepository)
        {
            _interfaceUNRepository = interfaceUNRepository;
        }
        //GET
        #region
        [HttpGet("GetAllUN")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> GetAllUN()
        {
            try
            {
                return Ok(await _interfaceUNRepository.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest(
                   new APIResponse
                   {
                       Success = false,
                       Message = $"Error GetAllUN: {ex.Message}"
                   });
            }
        }
        #endregion
    }
}
