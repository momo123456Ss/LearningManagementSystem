using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.ExemAndTest;
using LearningManagementSystem.Models.PaginatedList;
using LearningManagementSystem.Models.SubjectModel;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Security.Claims;

namespace LearningManagementSystem.Repository
{
    public class ExamAndTestRepository : InterfaceExamAndTest
    {
        private readonly LearningManagementSystemContext _context;
        private readonly Cloudinary _cloudinary;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _clientFactory;
        private static int PAGE_SIZE { get; set; } = 5;

        public ExamAndTestRepository(LearningManagementSystemContext context, Cloudinary cloudinary, IMapper mapper, IHttpContextAccessor httpContextAccessor, IHttpClientFactory clientFactory)
        {
            _context = context;
            _cloudinary = cloudinary;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _clientFactory = clientFactory;
        }
        //Private
        // Các kiểu mime tương ứng với các phần mở rộng tệp
        private static readonly Dictionary<string, string> MimeTypes = new Dictionary<string, string>
        {
            { ".pdf", "application/pdf" },
            { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { ".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
            { ".mp4", "video/mp4" }
            // Thêm các phần mở rộng và kiểu mime tương ứng của chúng nếu cần thiết
        };
        //GET
        #region
        public async Task<APIResponse> GetFileById(int fileId)
        {
            var fCloudinary = await _context.ExamAndTestS.FindAsync(fileId);
            if (fCloudinary == null)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "File not found"
                };
            }
            if (string.IsNullOrEmpty(fCloudinary.FileUrl))
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Missing Cloudinary Url"
                };
            }
            return new APIResponse
            {
                Success = false,
                Message = "Had found.",
                Data = _mapper.Map<ExamAndTestModelDowload>(fCloudinary)
            };
        }
        public async Task<byte[]> DowloadExamAndTestFile(string fileUrl)
        {
            // Kiểm tra xem đường dẫn Cloudinary đã được cung cấp chưa
            if (string.IsNullOrEmpty(fileUrl))
            {
                throw new ArgumentException("Missing Cloudinary URL");
            }

            // Tạo HTTP client để tải tệp từ Cloudinary
            var client = _clientFactory.CreateClient();

            // Thực hiện yêu cầu GET để tải tệp từ Cloudinary
            var response = await client.GetAsync(fileUrl);

            // Kiểm tra xem yêu cầu có thành công không
            if (response.IsSuccessStatusCode)
            {
                // Xác định kiểu mime dựa trên đuôi mở rộng của tệp
                var fileExtension = System.IO.Path.GetExtension(fileUrl).ToLower();
                var contentType = MimeTypes.GetValueOrDefault(fileExtension, "application/octet-stream");

                // Trả về nội dung tệp dưới dạng byte array
                return await response.Content.ReadAsByteArrayAsync();
            }
            else
            {
                // Ném ngoại lệ nếu yêu cầu không thành công
                throw new HttpRequestException($"Error downloading file from Cloudinary: {response.StatusCode}");
            }
        }

        public async Task<APIResponse> GetExamAndTestForTeacher(string searchString, string facultyId, string subjectId, int page = 1)
        {
            var user = await _context.Users.Include(role => role.UserRole)
                         .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
            if (!user.UserRole.ExamsAndTestsSee)
            {
                return new APIResponse
                {
                    Success = true,
                    Message = "Teacher not have permission to see.",
                };
            }
            var allExamAndTest = _context.ExamAndTestS
                .Include(s => s.SubjectNavigation)
                .Include(f => f.FacultyNavigation)
                .Where(eat => eat.SubjectNavigation.LecturerId.Equals(user.UserId))
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                allExamAndTest = allExamAndTest.Where(f =>
                    f.FileName.ToLower().Contains(searchString) ||
                    f.ExamForm.ToLower().Contains(searchString) ||
                    f.Status.ToLower().Contains(searchString)
                );
            }
            if (!string.IsNullOrEmpty(facultyId))
            {
                allExamAndTest = allExamAndTest.Where(f => f.FacultyId.Equals(Guid.Parse(facultyId)));
            }
            if (!string.IsNullOrEmpty(subjectId))
            {
                allExamAndTest = allExamAndTest.Where(f => f.SubjectId.Equals(Guid.Parse(subjectId)));
            }
            allExamAndTest = allExamAndTest.OrderBy(a => a.CreatedDate);

            if (page < 1)
            {
                return new APIResponse
                {
                    Success = true,
                    Message = "Had found.",
                    Data = _mapper.Map<List<ExamAndTestViewModel>>(allExamAndTest)
                };
            }
            var result = PaginatedList<ExamAndTest>.Create(allExamAndTest, page, PAGE_SIZE);
            return new APIResponse
            {
                Success = true,
                Message = "Had found.",
                Data = _mapper.Map<List<ExamAndTestViewModel>>(result)
            };
        }
        public async Task<APIResponse> GetExamAndTestForAdmin(string searchString, string subjectId, string teacherId, string status, int page = 1)
        {
            var user = await _context.Users.Include(role => role.UserRole)
                         .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
            if (!user.UserRole.ExamsAndTestsSee)
            {
                return new APIResponse
                {
                    Success = true,
                    Message = "Admin not have permission to see.",
                };
            }
            var allExamAndTest = _context.ExamAndTestS
                .Include(s => s.SubjectNavigation)
                    .ThenInclude(u => u.UserNavigation)
                .Include(f => f.FacultyNavigation)
                .Where(a => a.Status.Equals("Chờ phê duyệt"))
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                allExamAndTest = allExamAndTest.Where(f =>
                    f.FileName.ToLower().Contains(searchString) ||
                    f.ExamForm.ToLower().Contains(searchString) ||
                    f.Status.ToLower().Contains(searchString) ||
                    String.Concat(f.SubjectNavigation.UserNavigation.FirstName + " " + f.SubjectNavigation.UserNavigation.LastName).Contains(searchString)
                );
            }
            if (!string.IsNullOrEmpty(status))
            {
                status = status.ToLower();
                allExamAndTest = allExamAndTest.Where(f => f.Status.ToLower().Contains(status));
                    
            }
            if (!string.IsNullOrEmpty(subjectId))
            {
                allExamAndTest = allExamAndTest.Where(f => f.SubjectId.Equals(Guid.Parse(subjectId)));
            }
            if (!string.IsNullOrEmpty(teacherId))
            {
                allExamAndTest = allExamAndTest.Where(f => f.SubjectNavigation.LecturerId.Equals(Guid.Parse(teacherId)));
            }
            allExamAndTest = allExamAndTest.OrderBy(a => a.CreatedDate);

            if (page < 1)
            {
                return new APIResponse
                {
                    Success = true,
                    Message = "Had found.",
                    Data = _mapper.Map<List<ExamAndTestViewModel>>(allExamAndTest)
                };
            }
            var result = PaginatedList<ExamAndTest>.Create(allExamAndTest, page, PAGE_SIZE);
            return new APIResponse
            {
                Success = true,
                Message = "Had found.",
                Data = _mapper.Map<List<ExamAndTestViewModel>>(result)
            };
        }
        #endregion
        //POST
        #region
        public async Task<APIResponse> UploadExamAndTestFileEassy(ExamAndTestModelUploadFile model)
        {

            var subject = await _context.Subjects.FindAsync(model.SubjectId);
            if (subject == null)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Subject not found."
                };
            }
            var faculty = await _context.Facultys.FindAsync(model.FacultyId);
            if (faculty == null)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Faculty not found."
                };
            }
            //Upload Cloudinary
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(model.File.FileName, model.File.OpenReadStream()),
                Folder = "resources_of_lms_folder/exam_and_test_folder"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            // Save file info to database
            var fileInfo = new ExamAndTest
            {
                SubjectId = model.SubjectId,
                FacultyId = model.FacultyId,
                Time = model.Time,
                ExamForm = "Tự luận",
                Status = "Lưu nháp",
                CreatedDate = DateTime.Now,
                //file info
                FileType = Path.GetExtension(model.File.FileName),
                FileName = model.FileName,
                FileSize = $"{(double)model.File.Length / (1024 * 1024):0.##} MB",
                FileUrl = uploadResult.SecureUri.ToString(),
                FileViewUrl = (Path.GetExtension(model.File.FileName) == ".mp3" || Path.GetExtension(model.File.FileName) == ".mp4" || Path.GetExtension(model.File.FileName) == ".pdf")
                ? uploadResult.SecureUri.ToString() : String.Concat("https://docs.google.com/viewer?url=", uploadResult.SecureUri.ToString())
            };

            _context.ExamAndTestS.Add(fileInfo);
            await _context.SaveChangesAsync();
            return new APIResponse
            {
                Success = true,
                Message = "Upload file success."
            };
        }
        public async Task<APIResponse> UploadExamAndTestFileMultipleChoice(ExamAndTestModelUploadFile model)
        {
            var subject = await _context.Subjects.FindAsync(model.SubjectId);
            if (subject == null)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Subject not found."
                };
            }
            var faculty = await _context.Facultys.FindAsync(model.FacultyId);
            if (faculty == null)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Faculty not found."
                };
            }
            //Upload Cloudinary
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(model.File.FileName, model.File.OpenReadStream()),
                Folder = "resources_of_lms_folder/exam_and_test_folder"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            // Save file info to database
            var fileInfo = new ExamAndTest
            {
                SubjectId = model.SubjectId,
                FacultyId = model.FacultyId,
                Time = model.Time,
                ExamForm = "Trắc nghiệm",
                Status = "Lưu nháp",
                CreatedDate = DateTime.Now,
                //file info
                FileType = Path.GetExtension(model.File.FileName),
                FileName = model.FileName,
                FileSize = $"{(double)model.File.Length / (1024 * 1024):0.##} MB",
                FileUrl = uploadResult.SecureUri.ToString(),
                FileViewUrl = (Path.GetExtension(model.File.FileName) == ".mp3" || Path.GetExtension(model.File.FileName) == ".mp4" || Path.GetExtension(model.File.FileName) == ".pdf")
                ? uploadResult.SecureUri.ToString() : String.Concat("https://docs.google.com/viewer?url=", uploadResult.SecureUri.ToString())
            };

            _context.ExamAndTestS.Add(fileInfo);
            await _context.SaveChangesAsync();
            return new APIResponse
            {
                Success = true,
                Message = "Upload file success."
            };
        }
        #endregion
        //PUT
        #region
        public async Task<APIResponse> ChageFileName(string fileId, string newFileName)
        {
            try
            {
                var user = await _context.Users.Include(role => role.UserRole)
                          .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
                if (!user.UserRole.ExamsAndTestsEdit)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "User does not have permission to edit."
                    };
                }
                var fileFound = await _context.ExamAndTestS.FindAsync(int.Parse(fileId));
                if (fileFound == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = $"File not found."
                    };
                }
                fileFound.FileName = newFileName;
                fileFound.LastModifiedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return new APIResponse { Success = true, Message = "ChageFileName successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Error ChageFileName: {ex.Message}",
                };
            }
        }

        public async Task<APIResponse> ApproveFile(string fileId)
        {
            try
            {
                var user = await _context.Users.Include(role => role.UserRole)
                          .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
                if (!user.UserRole.ExamsAndTestsAcceptance)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "User does not have permission to edit."
                    };
                }
                var fileFound = await _context.ExamAndTestS.FindAsync(int.Parse(fileId));
                if (fileFound == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = $"File not found."
                    };
                }
                fileFound.Approve = true;
                fileFound.Status = "Đã phê duyệt";
                fileFound.Note = null;
                await _context.SaveChangesAsync();
                return new APIResponse { Success = true, Message = "ApproveFile successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Error ApproveFile: {ex.Message}",
                };
            }
        }

        public async Task<APIResponse> NotApproveFile(string fileId, string note)
        {
            try
            {
                var user = await _context.Users.Include(role => role.UserRole)
                          .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
                if (!user.UserRole.ExamsAndTestsAcceptance)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "User does not have permission to edit."
                    };
                }
                var fileFound = await _context.ExamAndTestS.FindAsync(int.Parse(fileId));
                if (fileFound == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = $"File not found."
                    };
                }
                fileFound.Approve = false;
                fileFound.Status = "Không phê duyệt";
                fileFound.Note = note;
                await _context.SaveChangesAsync();
                return new APIResponse { Success = true, Message = "NotApproveFile successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Error NotApproveFile: {ex.Message}",
                };
            }
        }

        public async Task<APIResponse> SendForApproval(string fileId)
        {
            try
            {
                var user = await _context.Users.Include(role => role.UserRole)
                          .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
                if (!user.UserRole.ExamsAndTestsEdit)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "User does not have permission to edit."
                    };
                }
                var fileFound = await _context.ExamAndTestS.FindAsync(int.Parse(fileId));
                if (fileFound == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = $"File not found."
                    };
                }
                fileFound.Status = "Chờ phê duyệt";
                await _context.SaveChangesAsync();
                return new APIResponse { Success = true, Message = "SendForApproval successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Error SendForApproval: {ex.Message}",
                };
            }
        }
        #endregion
        //DELETE
        #region
        public async Task<APIResponse> DeleteExamAndTestById(string fileId)
        {
            var user = await _context.Users.Include(role => role.UserRole)
                          .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
            if (!user.UserRole.ExamsAndTestsDelete)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "User does not have permission to edit."
                };
            }
            var fileFound = await _context.ExamAndTestS.FirstOrDefaultAsync(f => f.ExamAndTestId == int.Parse(fileId));
            //Xóa file cũ
            string currentFileUrl = fileFound.FileUrl;
            string publicId = string.Empty;
            string path = string.Empty;
            string fileName = string.Empty;
            if (!string.IsNullOrEmpty(currentFileUrl))
            {
                Uri uri = new Uri(currentFileUrl);
                path = uri.AbsolutePath;
                fileName = Path.GetFileName(path);
                publicId = Path.GetFileNameWithoutExtension(path);
            }
            // Sử dụng Cloudinary API để xóa hình ảnh sử dụng public ID
            if (!string.IsNullOrEmpty(publicId))
            {
                var deletionParams = new DelResParams()
                {
                    PublicIds = new List<string> { "resources_of_lms_folder/exam_and_test_folder/" + fileName },
                    Type = "upload",
                    ResourceType = ResourceType.Raw
                };

                var deletionResult = _cloudinary.DeleteResources(deletionParams);
                // Kiểm tra xem hình ảnh đã được xóa thành công hay không
                if (deletionResult != null && deletionResult.Deleted != null && deletionResult.Deleted.Count > 0)
                {
                    // Xóa hình ảnh thành công
                    _context.ExamAndTestS.Remove(fileFound);
                    await _context.SaveChangesAsync();
                    return new APIResponse { Success = false, Message = "Deleting current file success" };
                }
                else
                {
                    return new APIResponse { Success = false, Message = "Deleting current file failed" };
                }
            }
            return new APIResponse { Success = true, Message = "Something Error" };
        }


        #endregion
    }
}
