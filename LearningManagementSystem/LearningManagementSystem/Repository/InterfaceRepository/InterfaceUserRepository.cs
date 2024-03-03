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
        Task<APIResponse> SignIn(SignInModel model);
        Task<APIResponse> CreateLeadershipUser(UserModelAllType model);
        Task<APIResponse> UpdateUserPersonalInformation(string id,UserModelUpdateAllType model);
        Task<APIResponse> ChangeUserAvatar(UserModelChangeAvatar model);
        Task<APIResponse> RenewToken(TokenModel model);
        ClaimsPrincipal ValidateToken(string token);

        Task<APIResponse> ActiveUser(string id);
    }
}
