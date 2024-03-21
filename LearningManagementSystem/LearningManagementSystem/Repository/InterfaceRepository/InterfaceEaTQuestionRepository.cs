using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.ExamAndTestQuestionModel;

namespace LearningManagementSystem.Repository.InterfaceRepository
{
    public interface InterfaceEaTQuestionRepository
    {
        //GET
        #region
        Task<ExamAndTestQuestion1ViewModel> GetById(string id);
        Task<List<ExamAndTestQuestionViewModel>> GetAll(string? searchString, string? facultyId, string? subjectId, int? tier, int page = 1);
        #endregion
        //POST
        #region
        Task<APIResponse> GenerateExamExcel(string tendethi,string facultyId, string subjectId, string hinhthuc, string thoigianthi, int easy, int normal, int difficult);

        Task<byte[]> GenerateExamDocument(string facultyId, string subjectId,string made , string hinhthuc, string thoigianthi, int easy, int normal, int difficult);
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
