using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.ExamAndTestQuestionModel;

namespace LearningManagementSystem.Repository.InterfaceRepository
{
    public interface InterfaceEaTQuestionRepository
    {
        //GET
        #region
        #endregion
        //POST
        #region
        Task<APIResponse> CreateQuestion(ExamAndTestQuestionCreateModel model);
        Task<APIResponse> CreateQuestions(List<ExamAndTestQuestionCreateModel> models);

        #endregion
        //PUT
        #region
        Task<APIResponse> UpdateQuestion(string id,ExamAndTestQuestionUpdateModel model);

        #endregion
        //DELETE
        #region
        Task<APIResponse> DeleteQuestion(string id);

        #endregion
    }
}
