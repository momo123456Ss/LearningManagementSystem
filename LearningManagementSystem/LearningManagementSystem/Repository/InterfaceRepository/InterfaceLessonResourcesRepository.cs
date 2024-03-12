using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.LessonResources;

namespace LearningManagementSystem.Repository.InterfaceRepository
{
    public interface InterfaceLessonResourcesRepository
    {
        //GET
        #region
        Task<APIResponse> GetObjectByLessonId(string lessonId);
        Task<APIResponse> GetObjectByLessonIdAndClassId(string lessonId, string classId);
        Task<APIResponse> GetListLecturesAndResourcesIdBySubjectTopicIdAndClassId(int subjectTopicId, string classId);
        #endregion
        //POST
        #region
        Task<APIResponse> CreateNewLecturesAndAddFileToLecture(LessonLectureModelCreate model);
        Task<APIResponse> AddResourcesToLecture(LessonResourcesModelCreate model);
        Task<APIResponse> AddLectureAndFileToClasses(LessonResourcesModelCreate model);
        #endregion
        //PUT
        #region
        #endregion
        //DELETE
        #region
        #endregion
    }
}
