using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.SAModel;

namespace LearningManagementSystem.Repository.InterfaceRepository
{
    public interface InterfaceSARepository
    {
        //GET
        #region
        #endregion
        //POST
        #region
        Task<APIResponse> CreateSA(SubjectAnnouncementModelCreate model);
        #endregion
        //PUT
        #region
        #endregion
    }
}
