using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.SubjectModel;
using LearningManagementSystem.Models.UserRoleModels;

namespace LearningManagementSystem.Repository.InterfaceRepository
{
    public interface InterfaceSubjectRepository
    {
        //GET
        #region
        Task<List<SubjectModelView>> GetAll(string searchString, int page = 1);
        Task<APIResponse> GetById(string id);
        #endregion
        //POST
        #region
        Task<APIResponse> CreateNew(SubjectModelCreate model);
        #endregion
        //PUT
        #region
        Task<APIResponse> UpdateById(string id, SubjectModelUpdate model);
        #endregion
    }
}
