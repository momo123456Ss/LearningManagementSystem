using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.LessonModel;

namespace LearningManagementSystem.Repository.InterfaceRepository
{
    public interface InterfaceLessonRepository
    {
        //GET
        #region
        #endregion
        //POST
        #region
        Task<APIResponse> CreateLesson(LessonModelCreate model);
        #endregion
        //PUT
        #region
        #endregion
    }
}
