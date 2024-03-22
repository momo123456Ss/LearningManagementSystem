using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.LearningStatisticsModel;

namespace LearningManagementSystem.Repository.InterfaceRepository
{
    public interface InterfaceLearningStatisticsRepository
    {
        //GET
        Task<APIResponse> SubjectSpendTheMostTimeOn(string academicYear);
        Task<APIResponse> GetMinus(string time);
        //POST
        Task<APIResponse> TimeBegin(LearningStatisticsModelCreate model);
        //PUT
        Task<APIResponse> TimeEnd(string Id);

    }
}
