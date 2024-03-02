using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.TokenModel;
using LearningManagementSystem.Models.UserModel;
using System.Security.Claims;

namespace LearningManagementSystem.Repository.InterfaceRepository
{
    public interface InterfaceUserRepository
    {
        Task<APIResponse> SignIn(SignInModel model);
        Task<APIResponse> SignUp(SignUpModel model);

        Task<APIResponse> RenewToken(TokenModel model);
        ClaimsPrincipal ValidateToken(string token);
    }
}
