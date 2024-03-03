using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.TokenModel;
using LearningManagementSystem.Models.UserModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LearningManagementSystem.Repository.InterfaceRepository
{
    public interface InterfaceUserRepository
    {
        Task<List<UserViewModel>> GetAll(string searchString, string roleName,int page = 1);
        Task<APIResponse> SignIn(SignInModel model);
        Task<APIResponse> CreateLeadershipUser(UserModelAllType model);
        Task<APIResponse> CreateTeacherUser(UserModelAllType model);
        Task<APIResponse> CreateStudentUser(UserModelAllType model);

        Task<APIResponse> UpdateUserPersonalInformation(string id,UserModelUpdateAllType model);
        Task<APIResponse> ChangeUserAvatar(UserModelChangeAvatar model);
        Task<APIResponse> RenewToken(TokenModel model);
        ClaimsPrincipal ValidateToken(string token);

        Task<APIResponse> ActiveUser(string id);
        Task<APIResponse> ChangePassword(UserChangePasswordModel model);
        Task<UserViewModel> GetUserById(string id);
    }
}
