using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.UserRoleModels;

namespace LearningManagementSystem.Repository.InterfaceRepository
{
    public interface InterfaceUserRoleRepository
    {
        //GET
        Task<List<UserRoleViewModel>> GetAll();
        Task<APIResponse> GetById(string id);
        //POST
        Task<APIResponse> CreateNew(UserRoleModel model);
        //PUT
        Task<APIResponse> UpdateById(string id, UserRoleModel model);
        //DELETE
        Task<APIResponse> DeleteById(string id);
    }
}
