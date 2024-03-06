using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.OtherSubjectInformationModel;
using LearningManagementSystem.Models.SubjectModel;

namespace LearningManagementSystem.Repository.InterfaceRepository
{
    public interface InterfaceOtherSubjectInformationRepository
    {
        //GET
        #region
        #endregion
        //POST
        #region
        Task<APIResponse> CreateNew(OtherSubjectInformationModelCreate model);
        #endregion
        //PUT
        #region
        Task<APIResponse> UpdateById(string id, OtherSubjectInformationModelUpdate model);
        #endregion
    }
}
