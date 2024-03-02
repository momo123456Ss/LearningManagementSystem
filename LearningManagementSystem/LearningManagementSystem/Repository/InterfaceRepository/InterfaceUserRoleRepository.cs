using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.UserRoleModels;

namespace LearningManagementSystem.Repository.InterfaceRepository
{
    public interface InterfaceUserRoleRepository
    {
        Task<List<UserRoleViewModel>> GetAll();
        Task<APIResponse> GetById(string id);
        Task<APIResponse> CreateNew(UserRoleModel model);
        Task<APIResponse> DeleteById(string id);
        Task<APIResponse> UpdateById(string id, UserRoleModel model);
    }
}
