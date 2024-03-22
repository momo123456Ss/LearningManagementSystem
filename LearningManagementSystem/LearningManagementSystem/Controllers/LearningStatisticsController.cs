using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.LearningStatisticsModel;
using LearningManagementSystem.Models.LessonModel;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearningStatisticsController : ControllerBase
    {
        private readonly InterfaceLearningStatisticsRepository _interfaceLearningStatisticsRepository;
        public LearningStatisticsController(InterfaceLearningStatisticsRepository interfaceLearningStatisticsRepository)
        {
            _interfaceLearningStatisticsRepository = interfaceLearningStatisticsRepository;
        }
        [HttpGet("SubjectSpendTheMostTimeOn/{academicYear}")]
        [Authorize(Policy = "RequireStudent")]
        public async Task<IActionResult> SubjectSpendTheMostTimeOn(string academicYear)
        {
            try
            {
                return Ok(await _interfaceLearningStatisticsRepository.SubjectSpendTheMostTimeOn(academicYear));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error SubjectSpendTheMostTimeOn: {ex.Message}"
                });
            }
        }
        [HttpGet("GetMinus/{time}")]
        [Authorize(Policy = "RequireStudent")]
        public async Task<IActionResult> GetMinus(string time)
        {
            try
            {
                return Ok(await _interfaceLearningStatisticsRepository.GetMinus(time));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error GetMinus: {ex.Message}"
                });
            }
        }
        [HttpPost("TimeBegin")]
        [Authorize(Policy = "RequireStudent")]
        public async Task<IActionResult> TimeBegin([FromBody] LearningStatisticsModelCreate model)
        {
            try
            {
                return Ok(await _interfaceLearningStatisticsRepository.TimeBegin(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error TimeBegin: {ex.Message}"
                });
            }
        }
        [HttpPut("TimeEnd/{id}")]
        [Authorize(Policy = "RequireStudent")]
        public async Task<IActionResult> TimeEnd(string id)
        {
            try
            {
                return Ok(await _interfaceLearningStatisticsRepository.TimeEnd(id));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error TimeEnd: {ex.Message}"
                });
            }
        }
    }
}
