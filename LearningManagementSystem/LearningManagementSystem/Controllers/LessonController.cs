using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.LessonModel;
using LearningManagementSystem.Models.LessonResources;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly InterfaceLessonRepository _interfaceLessonRepository;
        public LessonController(InterfaceLessonRepository interfaceLessonRepository) {
            this._interfaceLessonRepository = interfaceLessonRepository;
        }
        [HttpPost("CreateLesson")]
        [Authorize(Policy = "RequireAdministratorAndTeacher")]
        public async Task<IActionResult> CreateLesson([FromBody] LessonModelCreate model)
        {
            try
            {
                return Ok(await _interfaceLessonRepository.CreateLesson(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error CreateLesson: {ex.Message}"
                });
            }
        }
    }
}
