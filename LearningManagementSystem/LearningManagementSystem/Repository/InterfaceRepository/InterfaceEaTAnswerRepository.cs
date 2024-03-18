using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.ExamAndTestQuestionModel.AnswerModel;

namespace LearningManagementSystem.Repository.InterfaceRepository
{
    public interface InterfaceEaTAnswerRepository
    {
        //POST
        Task<APIResponse> AddAnswer(ExamAndTestAnswerAddOrUpdateModel model);
        //PUT
        Task<APIResponse> UpdateAnswer(string answerId, ExamAndTestAnswerAddOrUpdateModel model);
        //DELETE
        Task<APIResponse> DeleteAnswer(string answerId);

    }
}
