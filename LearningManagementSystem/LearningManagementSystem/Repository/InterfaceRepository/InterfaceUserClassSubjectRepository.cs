using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.UserBelongToFacultyModel;
using LearningManagementSystem.Models.UserClassSubjectModel;

namespace LearningManagementSystem.Repository.InterfaceRepository
{
    public interface InterfaceUserClassSubjectRepository
    {
        //GET
        #region
        Task<APIResponse> GetSubjectStudent(string academicYear, string semester , string searchString, string sortBy, bool? mark, int page = 1);
        #endregion
        //POST
        #region
        Task<APIResponse> CreatorNew(UserClassSubjectModelCreate model);
        #endregion
        //PUT
        #region
        Task<APIResponse> UpdateLastRecent(string subjectId, string classId);
        Task<APIResponse> UpdateMark(string subjectId, string classId);
        #endregion
    }
}
