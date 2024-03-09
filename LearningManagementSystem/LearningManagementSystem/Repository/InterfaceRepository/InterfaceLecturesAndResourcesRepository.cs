using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.LecturesAndResourcesModel;

namespace LearningManagementSystem.Repository.InterfaceRepository
{
    public interface InterfaceLecturesAndResourcesRepository
    {
        //GET
        #region
        Task<Stream> DownloadFilesAsZip(List<int> fileIds);
        Task<byte[]> DowloadLecturesOrResources(string fileUrl);
        Task<APIResponse> GetFileById(int fileId);
        Task<APIResponse> GetLecturesAndResourcesByAdmin(string searchString, string sortBy, int page = 1);
        Task<APIResponse> GetLecturesFromInstructors(string subjectId,string searchString,string sortBy, int page = 1);
        Task<APIResponse> GetResourcesFromInstructors(string subjectId, string searchString, string sortBy, int page = 1);
        #endregion
        //POST
        #region
        Task<APIResponse> UploadLectureFile(LecturesAndResourcesModelCreate model);
        Task<APIResponse> UploadResourceFile(LecturesAndResourcesModelCreate model);
        #endregion
        //PUT
        #region
        Task<APIResponse> ChageFileName(string fileId, string newFileName);
        Task<APIResponse> ApproveFile(string fileId);
        Task<APIResponse> ApproveMultipleFile(List<string> fileId);
        Task<APIResponse> NotApproveFile(string fileId, string note);
        Task<APIResponse> NotApproveMultipleFile(List<string> fileId, string note);
        Task<APIResponse> ChageLectureFile(string fileId, IFormFile newFile);
        Task<APIResponse> ChageResourceFile(string fileId, IFormFile newFile);
        #endregion
        //DELETE
        #region
        #endregion
    }
}
