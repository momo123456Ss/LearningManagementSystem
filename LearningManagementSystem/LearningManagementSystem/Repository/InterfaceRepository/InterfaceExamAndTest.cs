using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.ExemAndTest;
using LearningManagementSystem.Models.LecturesAndResourcesModel;

namespace LearningManagementSystem.Repository.InterfaceRepository
{
    public interface InterfaceExamAndTest
    {
        //GET
        #region
        Task<APIResponse> GetExamAndTestForTeacher(string searchString, string facultyId, string subjectId, int page = 1);
        Task<APIResponse> GetExamAndTestForAdmin(string searchString, string subjectId
                                            ,string teacherId,string status ,int page = 1);
        Task<APIResponse> GetFileById(int fileId);
        Task<byte[]> DowloadExamAndTestFile(string fileUrl);
        #endregion
        //POST
        #region
        Task<APIResponse> UploadExamAndTestFileMultipleChoice(ExamAndTestModelUploadFile model);
        Task<APIResponse> UploadExamAndTestFileEassy(ExamAndTestModelUploadFile model);
        #endregion
        //PUT
        #region
        Task<APIResponse> ChageFileName(string fileId, string newFileName);
        Task<APIResponse> ApproveFile(string fileId);
        Task<APIResponse> NotApproveFile(string fileId, string note);
        Task<APIResponse> SendForApproval(string fileId);
        #endregion
        //DELETE
        #region
        Task<APIResponse> DeleteExamAndTestById(string fileId);
        #endregion
    }
}
