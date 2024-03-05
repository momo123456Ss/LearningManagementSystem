using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.ClassModel;
using LearningManagementSystem.Models.UserRoleModels;

namespace LearningManagementSystem.Repository.InterfaceRepository
{
    public interface InterfaceClassRepository
    {
        //GET
        #region

        Task<List<ClassModelView>> GetAll();
        Task<List<ClassModelView>> GetAllClassDoesNotHaveAnEndDateYet();
        Task<List<ClassModelView>> GetAllClassHasEnded();
        Task<List<ClassModelView>> GetAllClassHasNotYetEnded();
        Task<List<ClassModelView>> GetAllClassOpenDateStartBetween(string startString, string endString);

        Task<APIResponse> GetById(string id);
        #endregion
        //POST
        #region
        Task<APIResponse> CreateNewClass(ClassModelCreate model);
        #endregion
        //PUT
        #region
        Task<APIResponse> UpdateById(string id, ClassModelUpdate model);
        #endregion
    }
}
