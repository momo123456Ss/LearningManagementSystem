using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.LessonResources;
using LearningManagementSystem.Models.SubjectModel;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonResourcesController : ControllerBase
    {
        private readonly InterfaceLessonResourcesRepository _interfaceLessonResourcesRepository;
        private readonly InterfaceLecturesAndResourcesRepository _interfaceLecturesAndResourcesRepository;
        public LessonResourcesController(InterfaceLessonResourcesRepository interfaceLessonResourcesRepository, InterfaceLecturesAndResourcesRepository interfaceLecturesAndResourcesRepository)
        {
            this._interfaceLessonResourcesRepository = interfaceLessonResourcesRepository;
            this._interfaceLecturesAndResourcesRepository = interfaceLecturesAndResourcesRepository;
        }

        //GET
        #region
        [HttpGet("GetObjectByLessonId/{lessonId}")]
        [Authorize(Policy = "RequireAdministratorAndTeacher")]
        public async Task<IActionResult> GetObjectByLessonIdAndClassId(string lessonId)
        {
            try
            {
                return Ok(await _interfaceLessonResourcesRepository.GetObjectByLessonId(lessonId));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error GetObjectByLessonIdAndClassId: {ex.Message}"
                });
            }
        }
        [HttpGet("GetObjectByLessonIdAndClassId/{lessonId}/class/{classId}")]
        [Authorize (Policy = "RequireStudent")]
        public async Task<IActionResult> GetObjectByLessonIdAndClassId(string lessonId, string classId)
        {
            try
            {
                return Ok(await _interfaceLessonResourcesRepository.GetObjectByLessonIdAndClassId(lessonId, classId));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error GetObjectByLessonIdAndClassId: {ex.Message}"
                });
            }
        }
        [HttpGet("GetAllFileBySubjectTopIcId/{subjectTopicId}/class{classId}")]
        [Authorize]
        public async Task<IActionResult> GetAllFileBySubjectTopIcId(int subjectTopicId, string classId)
        {
            try
            {
                var apiResponse = await _interfaceLessonResourcesRepository.GetListLecturesAndResourcesIdBySubjectTopicIdAndClassId(subjectTopicId, classId);
                var fileIdAndTopicTitle = apiResponse.Data as FileIdAndTopicTitle;
                return File(await _interfaceLecturesAndResourcesRepository.DownloadFilesAsZip(fileIdAndTopicTitle.FileId), "application/zip", $"{fileIdAndTopicTitle.TopicTitle}_LMS_{DateTime.Now:yyyyMMddHHmmss}.zip");
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error GetAllFileBySubjectTopIcId: {ex.Message}"
                });
            }
        }
        #endregion
        //POST
        #region
        [HttpPost("CreateNewLecturesAndAddFileToLecture")]
        public async Task<IActionResult> CreateNewLecturesAndAddFileToLecture([FromBody] LessonLectureModelCreate model)
        {
            try
            {
                return Ok(await _interfaceLessonResourcesRepository.CreateNewLecturesAndAddFileToLecture(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error AddLecturesToTheSubject: {ex.Message}"
                });
            }
        }
        [HttpPost("AddResourcesToLecture")]
        public async Task<IActionResult> AddResourcesToLecture([FromBody] LessonResourcesModelCreate model)
        {
            try
            {
                return Ok(await _interfaceLessonResourcesRepository.AddResourcesToLecture(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error AddLecturesToTheSubject: {ex.Message}"
                });
            }
        }
        [HttpPost("AddLectureAndFileToClasses")]
        public async Task<IActionResult> AddLectureAndFileToClasses([FromBody] LessonResourcesModelCreate model)
        {
            try
            {
                return Ok(await _interfaceLessonResourcesRepository.AddLectureAndFileToClasses(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error AddLecturesToTheSubject: {ex.Message}"
                });
            }
        }
        #endregion
    }
}
