using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.SubjectModel;
using LearningManagementSystem.Models.SubjectTopicModel;

namespace LearningManagementSystem.Repository.InterfaceRepository
{
    public interface InterfaceSubjectTopicRepository
    {
        //GET
        #region
        Task<APIResponse> GetById(string id);
        #endregion
        //POST
        #region
        Task<APIResponse> CreateNew(SubjectTopicModelCreate model);
        #endregion
        //PUT
        #region
        Task<APIResponse> UpdateById(string id, SubjectTopicModelUpdate model);
        #endregion
        //DELETE
        #region
        Task<APIResponse> DeleteById(string id);
        #endregion
    }
}
