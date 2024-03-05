using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.UserBelongToFacultyModel;

namespace LearningManagementSystem.Repository.InterfaceRepository
{
    public interface InterfaceUserBelongToFacultyRepository
    {
        //Get
        #region
        #endregion
        //Post
        #region
        Task<APIResponse> CreatorUserBelongToFaculty(UserBelongToFacultyModelCreate model);
        #endregion
        //Put
        #region
        Task<APIResponse> SetHeadOfDepartmentByUserIdAndFacultyId(string userId, string facultyId);
        #endregion
    }
}
