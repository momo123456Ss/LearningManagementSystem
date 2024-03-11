using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.LecturesAndResourcesModel;
using LearningManagementSystem.Models.SubjectModel;
using LearningManagementSystem.Models.UserModel;
using LearningManagementSystem.Repository;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net.Mime;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturesAndResourcesController : ControllerBase
    {
        private readonly InterfaceLecturesAndResourcesRepository _interfaceLecturesAndResourcesRepository;
        private readonly LearningManagementSystemContext _context;

        public LecturesAndResourcesController(InterfaceLecturesAndResourcesRepository interfaceLecturesAndResourcesRepository, LearningManagementSystemContext context)
        {
            this._interfaceLecturesAndResourcesRepository = interfaceLecturesAndResourcesRepository;
            this._context = context;
        }
        //Private
        private string GetContentType(string cloudinaryUrl)
        {
            var fileExtension = Path.GetExtension(cloudinaryUrl).ToLower();
            switch (fileExtension)
            {
                case ".pdf":
                    return "application/pdf";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".pptx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case ".mp4":
                    return "video/mp4";
                default:
                    return MediaTypeNames.Application.Octet;
            }
        }
        //GET
        #region       
        [HttpGet("DowloadLecturesOrResources/{id}")]
        public async Task<IActionResult> DowloadLecturesOrResources(string id)
        {
            try
            {
                var response = await _interfaceLecturesAndResourcesRepository.GetFileById(int.Parse(id));
                var fileData = response.Data as LecturesAndResourcesModelDowload;
                return File(await _interfaceLecturesAndResourcesRepository.DowloadLecturesOrResources(fileData.FileUrl)
                    , GetContentType(fileData.FileUrl), fileData.FileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        [HttpGet("GetLecturesAndResourcesByAdmin")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> GetLecturesAndResourcesByAdmin(string? searchString, string? sortBy, int page = 1)
        {
            try
            {
                return Ok(await _interfaceLecturesAndResourcesRepository.GetLecturesAndResourcesByAdmin(searchString, sortBy, page));
            }
            catch (Exception ex)
            {
                return BadRequest(
                   new APIResponse
                   {
                       Success = false,
                       Message = $"Error GetLecturesFromInstructors: {ex.Message}"
                   });
            }
        }
        [HttpGet("GetLecturesFromInstructors/{subjectId}")]
        [Authorize(Policy = "RequireTeacher")]
        public async Task<IActionResult> GetLecturesFromInstructors(string subjectId, string? searchString, string? sortBy, int page = 1)
        {
            try
            {
                return Ok(await _interfaceLecturesAndResourcesRepository.GetLecturesFromInstructors(subjectId, searchString, sortBy, page));
            }
            catch (Exception ex)
            {
                return BadRequest(
                   new APIResponse
                   {
                       Success = false,
                       Message = $"Error GetLecturesFromInstructors: {ex.Message}"
                   });
            }
        }
        [HttpGet("GetResourcesFromInstructors/{subjectId}")]
        [Authorize(Policy = "RequireTeacher")]
        public async Task<IActionResult> GetResourcesFromInstructors(string subjectId, string? searchString, string? sortBy, int page = 1)
        {
            try
            {
                return Ok(await _interfaceLecturesAndResourcesRepository.GetResourcesFromInstructors(subjectId, searchString, sortBy, page));
            }
            catch (Exception ex)
            {
                return BadRequest(
                   new APIResponse
                   {
                       Success = false,
                       Message = $"Error GetResourcesFromInstructors: {ex.Message}"
                   });
            }
        }
        #endregion
        //POST
        #region
        [HttpPost("DowloadLecturesOrResources")]
        [Authorize]
        public async Task<IActionResult> DownloadFilesAsZip(List<int> fileIds)
        {
            try
            {
                return File(await _interfaceLecturesAndResourcesRepository.DownloadFilesAsZip(fileIds), "application/zip", $"cloudinary_files_LMS_{DateTime.Now:yyyyMMddHHmmss}.zip");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        [HttpPost("UploadLectureFile")]
        [Authorize]
        public async Task<IActionResult> UploadLectureFile([FromForm] LecturesAndResourcesModelCreate model)
        {
            try
            {
                return Ok(await _interfaceLecturesAndResourcesRepository.UploadLectureFile(model));
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new APIResponse
                    {
                        Success = false,
                        Message = $"Error UploadLectureFile: {ex.Message}"
                    });
            }
        }
        [HttpPost("UploadResoucerFile")]
        [Authorize]
        public async Task<IActionResult> UploadResoucerFile([FromForm] LecturesAndResourcesModelCreate model)
        {
            try
            {
                return Ok(await _interfaceLecturesAndResourcesRepository.UploadResourceFile(model));
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new APIResponse
                    {
                        Success = false,
                        Message = $"Error UploadResoucerFile: {ex.Message}"
                    });
            }
        }
        #endregion
        //PUT
        #region
        [HttpPut("ChageFileName/{fileId}/new-file-name/{newFileName}")]
        [Authorize(Policy = "RequireAdministratorAndTeacher")]
        public async Task<IActionResult> ChageFileName(string fileId, string newFileName)
        {
            try
            {
                return Ok(await _interfaceLecturesAndResourcesRepository.ChageFileName(fileId, newFileName));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error ChageFileName: {ex.Message}"
                });
            }
        }
        [HttpPut("ApproveFile/{fileId}")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> ApproveFile(string fileId)
        {
            try
            {
                return Ok(await _interfaceLecturesAndResourcesRepository.ApproveFile(fileId));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error ApproveFile: {ex.Message}"
                });
            }
        }
        [HttpPut("ApproveMultipleFile")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> ApproveMultipleFile(List<string> fileId)
        {
            try
            {
                return Ok(await _interfaceLecturesAndResourcesRepository.ApproveMultipleFile(fileId));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error ApproveMultipleFile: {ex.Message}"
                });
            }
        }
        [HttpPut("NotApproveFile/{fileId}/note/{note}")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> NotApproveFile(string fileId, string note)
        {
            try
            {
                return Ok(await _interfaceLecturesAndResourcesRepository.NotApproveFile(fileId,note));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error NotApproveFile: {ex.Message}"
                });
            }
        }
        [HttpPut("NotApproveMultipleFile/{note}")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> NotApproveMultipleFile(List<string> fileId, string note)
        {
            try
            {
                return Ok(await _interfaceLecturesAndResourcesRepository.NotApproveMultipleFile(fileId, note));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error NotApproveMultipleFile: {ex.Message}"
                });
            }
        }
        #endregion
        [HttpPut("ChageLectureFile/{fileId}")]
        [Authorize(Policy = "RequireAdministratorAndTeacher")]
        public async Task<IActionResult> ChageLectureFile(string fileId, IFormFile newFile)
        {
            try
            {
                return Ok(await _interfaceLecturesAndResourcesRepository.ChageLectureFile(fileId, newFile));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error ChageLectureFile: {ex.Message}"
                });
            }
        }
        [HttpPut("ChageResourceFile/{fileId}")]
        [Authorize(Policy = "RequireAdministratorAndTeacher")]
        public async Task<IActionResult> ChageResourceFile(string fileId, IFormFile newFile)
        {
            try
            {
                return Ok(await _interfaceLecturesAndResourcesRepository.ChageResourceFile(fileId, newFile));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error ChageLectureFile: {ex.Message}"
                });
            }
        }
        //DELETE
        #region
        #endregion
    }
}