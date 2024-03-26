using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.ExemAndTest;
using LearningManagementSystem.Models.LecturesAndResourcesModel;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamAndTestController : ControllerBase
    {
        private readonly InterfaceExamAndTest _interfaceExamAndTest;
        public ExamAndTestController(InterfaceExamAndTest interfaceExamAndTest)
        {
            this._interfaceExamAndTest = interfaceExamAndTest;
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
        [HttpGet("GetExamAndTestForTeacher")]
        [Authorize(Policy = "RequireTeacher")]
        public async Task<IActionResult> GetExamAndTestForTeacher(string? searchString, string? facultyId, string? subjectId, int page = 1)
        {
            try
            {
                return Ok(await _interfaceExamAndTest.GetExamAndTestForTeacher(searchString,facultyId,subjectId,page));
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new APIResponse
                    {
                        Success = false,
                        Message = $"Error GetExamAndTestForTeacher: {ex.Message}"
                    });
            }
        }
        [HttpGet("GetExamAndTestForAdmin")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> GetExamAndTestForAdmin(string? searchString, string? subjectId, string? teacherId, string? status, int page = 1)
        {
            try
            {
                return Ok(await _interfaceExamAndTest.GetExamAndTestForAdmin(searchString, subjectId, teacherId,status, page));
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new APIResponse
                    {
                        Success = false,
                        Message = $"Error GetExamAndTestForAdmin: {ex.Message}"
                    });
            }
        }
        [HttpGet("DowloadExamAndTestFile/{id}")]
        [Authorize(Policy = "RequireAdministratorAndTeacher")]
        public async Task<IActionResult> DowloadExamAndTestFile(string id)
        {
            try
            {
                var response = await _interfaceExamAndTest.GetFileById(int.Parse(id));
                var fileData = response.Data as ExamAndTestModelDowload;
                return File(await _interfaceExamAndTest.DowloadExamAndTestFile(fileData.FileUrl)
                    , GetContentType(fileData.FileUrl), fileData.FileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        #endregion
        //POST
        #region
        [HttpPost("UploadExamAndTestFileEassy")]
        [Authorize(Policy = "RequireAdministratorAndTeacher")]
        public async Task<IActionResult> UploadExamAndTestFileEassy([FromForm] ExamAndTestModelUploadFile model)
        {
            try
            {
                return Ok(await _interfaceExamAndTest.UploadExamAndTestFileEassy(model));
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new APIResponse
                    {
                        Success = false,
                        Message = $"Error UploadExamAndTestFileEassy: {ex.Message}"
                    });
            }
        }
        [HttpPost("UploadExamAndTestFileMultipleChoice")]
        [Authorize(Policy = "RequireAdministratorAndTeacher")]
        public async Task<IActionResult> UploadExamAndTestFileMultipleChoice([FromForm] ExamAndTestModelUploadFile model)
        {
            try
            {
                return Ok(await _interfaceExamAndTest.UploadExamAndTestFileMultipleChoice(model));
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new APIResponse
                    {
                        Success = false,
                        Message = $"Error UploadExamAndTestFileMultipleChoice: {ex.Message}"
                    });
            }
        }
        #endregion
        //PUT
        #region
        [HttpPut("ChageExamAndTestFileName/{fileId}/new-file-name/{newFileName}")]
        [Authorize(Policy = "RequireAdministratorAndTeacher")]
        public async Task<IActionResult> ChageExamAndTestFileName(string fileId, string newFileName)
        {
            try
            {
                return Ok(await _interfaceExamAndTest.ChageFileName(fileId, newFileName));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error ChageExamAndTestFileName: {ex.Message}"
                });
            }
        }
        [HttpPut("ApproveExamAndTestFile/{fileId}")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> ApproveExamAndTestFile(string fileId)
        {
            try
            {
                return Ok(await _interfaceExamAndTest.ApproveFile(fileId));
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
        [HttpPut("NotApproveExamAndTestFile/{fileId}/note/{note}")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> NotApproveExamAndTestFile(string fileId, string note)
        {
            try
            {
                return Ok(await _interfaceExamAndTest.NotApproveFile(fileId, note));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error NotApproveExamAndTestFile: {ex.Message}"
                });
            }
        }
        [HttpPut("SendApproveExamAndTestFile/{fileId}")]
        [Authorize(Policy = "RequireTeacher")]
        public async Task<IActionResult> SendApproveExamAndTestFile(string fileId)
        {
            try
            {
                return Ok(await _interfaceExamAndTest.SendForApproval(fileId));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error SendApproveExamAndTestFile: {ex.Message}"
                });
            }
        }
        #endregion
        //DELETE
        #region
        [HttpDelete("DeleteExamAndTestFile/{fileId}")]
        [Authorize(Policy = "RequireAdministratorAndTeacher")]
        public async Task<IActionResult> DeleteExamAndTestFile(string fileId)
        {
            try
            {
                return Ok(await _interfaceExamAndTest.DeleteExamAndTestById(fileId));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error DeleteExamAndTestFile: {ex.Message}"
                });
            }
        }
        #endregion
    }
}
