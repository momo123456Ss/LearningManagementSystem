using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.SAModel;

namespace LearningManagementSystem.Repository.InterfaceRepository
{
    public interface InterfaceSARepository
    {
        //GET
        #region
        Task<APIResponse> GetSA(string? subjectId, string? classId, bool isNotice, int page = 1);
        #endregion
        //POST
        #region
        Task<APIResponse> CreateSA(SubjectAnnouncementModelCreate model);
        Task<APIResponse> CreateSA(SubjectAnnouncementModelCreateSingle model);

        #endregion
        //PUT
        #region
        #endregion
    }
}
