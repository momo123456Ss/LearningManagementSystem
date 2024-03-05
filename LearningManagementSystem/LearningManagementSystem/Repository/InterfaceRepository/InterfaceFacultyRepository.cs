using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.FacultyModel;
using LearningManagementSystem.Models.UserRoleModels;

namespace LearningManagementSystem.Repository.InterfaceRepository
{
    public interface InterfaceFacultyRepository
    {
        //GET
        #region
        Task<List<FacultyModelView>> GetAll(string searchString, string sortBy, int page = 1);
        Task<APIResponse> GetById(string id);
        #endregion
        //POST
        #region
        Task<APIResponse> CreateNew(FacultyModelCreate model);
        #endregion
        //PUT
        #region
        Task<APIResponse> UpdateById(string id, FacultyModelUpdate model);
        #endregion
    }
}
