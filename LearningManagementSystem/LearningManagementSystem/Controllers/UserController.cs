using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.TokenModel;
using LearningManagementSystem.Models.UserModel;
using LearningManagementSystem.Models.UserRoleModels;
using LearningManagementSystem.Repository;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using System.Security.Claims;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly InterfaceUserRepository _interfaceUserRepository;
        private readonly IHttpClientFactory _clientFactory;


        public UserController(InterfaceUserRepository interfaceUserRepository, IHttpClientFactory clientFactory = null)
        {
            this._interfaceUserRepository = interfaceUserRepository;
            _clientFactory = clientFactory;
        }

        //GET
        #region
        [HttpGet("DownloadFileFromCloudinary")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> DownloadFile(string cloudinaryUrl)
        {
            // Kiểm tra xem đường dẫn Cloudinary đã được cung cấp chưa
            if (string.IsNullOrEmpty(cloudinaryUrl))
            {
                return BadRequest("Missing Cloudinary URL");
            }

            // Tạo HTTP client để tải tệp từ Cloudinary
            var client = _clientFactory.CreateClient();

            // Thực hiện yêu cầu GET để tải tệp từ Cloudinary
            var response = await client.GetAsync(cloudinaryUrl);

            // Kiểm tra xem yêu cầu có thành công không
            if (response.IsSuccessStatusCode)
            {
                // Trả về nội dung tệp dưới dạng byte array
                var fileBytes = await response.Content.ReadAsByteArrayAsync();

                // Xác định kiểu mime dựa trên đuôi mở rộng của tệp
                var fileExtension = System.IO.Path.GetExtension(cloudinaryUrl).ToLower();
                var contentType = MediaTypeNames.Application.Octet;

                switch (fileExtension)
                {
                    case ".pdf":
                        contentType = "application/pdf";
                        break;
                    case ".docx":
                        contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                    case ".xlsx":
                        contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        break;
                    case ".pptx":
                        contentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                        break;
                    case ".mp4":
                        contentType = "video/mp4";
                        break;
                        // Thêm các trường hợp khác tùy thuộc vào loại tệp bạn muốn hỗ trợ
                }

                // Trả về tệp dưới dạng file với kiểu mime tương ứng
                return File(fileBytes, contentType, "filename" + fileExtension);
            }
            else
            {
                // Trả về lỗi nếu yêu cầu không thành công
                return StatusCode((int)response.StatusCode, "Error downloading file from Cloudinary");
            }
        }
        [HttpGet("GetAllUser")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> GettAllUser(string? userCodeOrEmailOrFullname, string? roleName, int page = 1)
        {
            try
            {
                return Ok(await _interfaceUserRepository.GetAll(userCodeOrEmailOrFullname, roleName, page));
            } catch (Exception ex)
            {
                return BadRequest(
                   new APIResponse
                   {
                       Success = false,
                       Message = $"Error GettAllUser: {ex.Message}"
                   });
            }
        }
        [HttpGet("GetUserById/{id}")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                return Ok(await _interfaceUserRepository.GetUserById(id));
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
        #endregion
        //POST
        #region
        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenModel model)
        {
            try
            {
                return Ok(await _interfaceUserRepository.RenewToken(model));
            }
            catch (Exception ex) {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = $"Error RenewToken: {ex.Message}"
                });
            }
        }
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            try
            {
                return Ok(await _interfaceUserRepository.SignIn(model));
            } catch (Exception ex)
            {
                return BadRequest(
                    new APIResponse
                    {
                        Success = false,
                        Message = $"Error CreateLeadershipUser: {ex.Message}"
                    });
            }
        }
        [HttpPost("CreateLeadershipUser")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> CreateLeadershipUser([FromForm] UserModelAllType model)
        {
            try
            {
                return Ok(await _interfaceUserRepository.CreateLeadershipUser(model));
            } catch (Exception ex)
            {
                return BadRequest(
                    new APIResponse {
                        Success = false,
                        Message = $"Error CreateLeadershipUser: {ex.Message}"
                    });
            }
        }
        [HttpPost("CreateTeacherUser")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> CreateTeacherUser([FromForm] UserModelAllType model)
        {
            try
            {
                return Ok(await _interfaceUserRepository.CreateTeacherUser(model));
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new APIResponse
                    {
                        Success = false,
                        Message = $"Error CreateTeacherUser: {ex.Message}"
                    });
            }
        }
        [HttpPost("CreateStudentUser")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> CreateStudentUser([FromForm] UserModelAllType model)
        {
            try
            {
                return Ok(await _interfaceUserRepository.CreateStudentUser(model));
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new APIResponse
                    {
                        Success = false,
                        Message = $"Error CreateStudentUser: {ex.Message}"
                    });
            }
        }
        #endregion
        //PUT
        #region
        [HttpPut("UpdateUserPersonalInformation/{id}")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> UpdateUserPersonalInformation(string id, [FromBody] UserModelUpdateAllType model)
        {
            try
            {
                return Ok(await _interfaceUserRepository.UpdateUserPersonalInformation(id, model));
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new APIResponse
                    {
                        Success = false,
                        Message = $"Error UpdateUserPersonalInformation: {ex.Message}"
                    });
            }
        }
        [HttpPut("UpdateLeadershipModelUpdateNotificationSettings")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> UpdateLeadershipModelUpdateNotificationSettings([FromBody] LeadershipModelUpdateNotificationSettings model)
        {
            try
            {
                return Ok(await _interfaceUserRepository.UpdateLeadershipModelUpdateNotificationSettings(model));
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new APIResponse
                    {
                        Success = false,
                        Message = $"Error UpdateLeadershipModelUpdateNotificationSettings: {ex.Message}"
                    });
            }
        }
        [HttpPut("UpdateTeacherModelUpdateNotificationSettings")]
        [Authorize(Policy = "RequireTeacher")]
        public async Task<IActionResult> UpdateTeacherModelUpdateNotificationSettings([FromBody] TeacherModelUpdateNotificationSettings model)
        {
            try
            {
                return Ok(await _interfaceUserRepository.UpdateTeacherUpdateNotificationSettings(model));
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new APIResponse
                    {
                        Success = false,
                        Message = $"Error UpdateTeacherModelUpdateNotificationSettings: {ex.Message}"
                    });
            }
        }
        [HttpPut("UpdateStudentModelUpdateNotificationSettings")]
        [Authorize(Policy = "RequireStudent")]
        public async Task<IActionResult> UpdateStudentModelUpdateNotificationSettings([FromBody] StudentModelUpdateNotificationSettings model)
        {
            try
            {
                return Ok(await _interfaceUserRepository.UpdateStudentModelUpdateNotificationSettings(model));
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new APIResponse
                    {
                        Success = false,
                        Message = $"Error UpdateStudentModelUpdateNotificationSettings: {ex.Message}"
                    });
            }
        }
        [HttpPut("ActiveUser/{id}")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> ActiveUser(string id)
        {
            try
            {
                return Ok(await _interfaceUserRepository.ActiveUser(id));
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new APIResponse
                    {
                        Success = false,
                        Message = $"Error ActiveUser: {ex.Message}"
                    });
            }
        }
        [HttpPut("ChangeUserAvatar")]
        [Authorize]
        public async Task<IActionResult> ChangeUserAvatar([FromForm] UserModelChangeAvatar modelChangeAvatar)
        {
            try
            {
                return Ok(await _interfaceUserRepository.ChangeUserAvatar(modelChangeAvatar));
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new APIResponse
                    {
                        Success = false,
                        Message = $"Error ChangeUserAvatar: {ex.Message}"
                    });
            }
        }
        [HttpPut("ChangeUserPassword")]
        [Authorize]
        public async Task<IActionResult> ChangeUserPassword([FromBody] UserChangePasswordModel model)
        {
            try
            {
                return Ok(await _interfaceUserRepository.ChangePassword(model));
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new APIResponse
                    {
                        Success = false,
                        Message = $"Error ChangeUserPassword: {ex.Message}"
                    });
            }
        }
        #endregion
    }
}
