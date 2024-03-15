using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.UserNotificationsModel;

namespace LearningManagementSystem.Repository.InterfaceRepository
{
    public interface InterfaceUNRepository
    {
        //GET
        #region
        Task<APIResponse> GetAll();
        #endregion
        //POST
        #region
        Task CreateUN(UserNotificationModelCreate model);
        #endregion
        //PUT
    }
}
