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
        //GET
        Task<List<UserViewModel>> GetAll(string searchString, string roleName,int page = 1);
        Task<UserViewModel> GetUserById(string id);
        ClaimsPrincipal ValidateToken(string token);

        //POST
        Task<APIResponse> RenewToken(TokenModel model);
        Task<APIResponse> SignIn(SignInModel model);
        Task<APIResponse> CreateLeadershipUser(UserModelAllType model);
        Task<APIResponse> CreateTeacherUser(UserModelAllType model);
        Task<APIResponse> CreateStudentUser(UserModelAllType model);       
        //PUT
        Task<APIResponse> UpdateUserPersonalInformation(string id,UserModelUpdateAllType model);
        Task<APIResponse> UpdateLeadershipModelUpdateNotificationSettings(LeadershipModelUpdateNotificationSettings model);
        Task<APIResponse> UpdateTeacherUpdateNotificationSettings(TeacherModelUpdateNotificationSettings model);
        Task<APIResponse> UpdateStudentModelUpdateNotificationSettings(StudentModelUpdateNotificationSettings model);
        Task<APIResponse> ActiveUser(string id);
        Task<APIResponse> ChangeUserAvatar(UserModelChangeAvatar model);
        Task<APIResponse> ChangePassword(UserChangePasswordModel model);
        




    }
}
