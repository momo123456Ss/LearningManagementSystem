using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.TokenModel;
using LearningManagementSystem.Models.UserModel;
using LearningManagementSystem.Models.UserRoleModels;
using LearningManagementSystem.Repository;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly InterfaceUserRepository _interfaceUserRepository;

        public UserController(InterfaceUserRepository interfaceUserRepository) {
            this._interfaceUserRepository = interfaceUserRepository;
        }

        //GET

        //POST
        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenModel model)
        {
            try
            {
                return Ok(await _interfaceUserRepository.RenewToken(model));
            }
            catch(Exception ex){
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
            }catch (Exception ex)
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
            }catch (Exception ex)
            {
                return BadRequest(
                    new APIResponse { 
                        Success = false, 
                        Message = $"Error CreateLeadershipUser: {ex.Message}" 
                    });
            }
        }
        //PUT
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
                        Message = $"Error ActiveUser: {ex.Message}"
                    });
            }
        }
    }
}
