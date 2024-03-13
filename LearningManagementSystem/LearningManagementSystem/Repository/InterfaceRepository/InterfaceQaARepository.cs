using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.QaAModel;
using LearningManagementSystem.Models.SubjectModel;

namespace LearningManagementSystem.Repository.InterfaceRepository
{
    public interface InterfaceQaARepository
    {
        //GET
        #region
        Task<List<QaAModelUserView>> GetQaA(string? subjectId, string? classId,int? subjectTopicId, int? lessonId
                                , string? sortByCreatedDate
                                , bool QaAIsFollow
                                , bool myQuestions, bool QuestionsNoAnswer, int page = 1);
        #endregion
        //POST
        #region
        Task<APIResponse> CreateQaA(QaAModelCreate model);
        Task<APIResponse> QaAFollow(int QaAId);
        #endregion
        //PUT
        #region
        Task<APIResponse> QaALike(int QaAId);
        #endregion
    }
}
