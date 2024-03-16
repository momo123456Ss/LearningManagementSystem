using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.LecturesAndResourcesModel;
using LearningManagementSystem.Repository.InterfaceRepository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using LearningManagementSystem.Models.PaginatedList;
using System.IO.Compression;

namespace LearningManagementSystem.Repository
{
    public class LecturesAndResourcesRepository : InterfaceLecturesAndResourcesRepository
    {
        private readonly LearningManagementSystemContext _context;
        private readonly Cloudinary _cloudinary;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _clientFactory;

        private static int PAGE_SIZE { get; set; } = 5;

        public LecturesAndResourcesRepository(LearningManagementSystemContext context
            , Cloudinary cloudinary, IMapper mapper, IHttpContextAccessor httpContextAccessor
            , IHttpClientFactory clientFactory)
        {
            this._context = context;
            this._cloudinary = cloudinary;
            this._mapper = mapper;
            this._httpContextAccessor = httpContextAccessor;
            this._clientFactory = clientFactory;
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
            var fCloudinary = await _context.LecturesAndResourcesL.FindAsync(fileId);
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
                Data = _mapper.Map<LecturesAndResourcesModelDowload>(fCloudinary)
            };
        }
        public async Task<Stream> DownloadFilesAsZip(List<int> fileIds)
        {
            // Tạo một bộ đệm để lưu trữ dữ liệu của file zip
            MemoryStream outputStream = new MemoryStream();

            // Tạo một file zip
            using (ZipArchive zipArchive = new ZipArchive(outputStream, ZipArchiveMode.Create, true))
            {
                foreach (int fileId in fileIds)
                {
                    // Lấy thông tin file từ CSDL
                    var fileInfo = await _context.LecturesAndResourcesL.FindAsync(fileId);
                    if (fileInfo == null)
                    {
                        // Bỏ qua nếu không tìm thấy thông tin file
                        continue;
                    }

                    // Tải dữ liệu từ Cloudinary
                    var fileBytes = await DowloadLecturesOrResources(fileInfo.FileUrl);

                    // Thêm file vào file zip với tên file được lấy từ CSDL
                    var entry = zipArchive.CreateEntry(fileInfo.FileName);
                    using (var entryStream = entry.Open())
                    {
                        entryStream.Write(fileBytes, 0, fileBytes.Length);
                    }
                }
            }

            // Đặt con trỏ đọc của outputStream ở đầu để đảm bảo tất cả dữ liệu đã được ghi vào outputStream
            outputStream.Seek(0, SeekOrigin.Begin);

            return outputStream;
        }
        public async Task<byte[]> DowloadLecturesOrResources(string fileUrl)
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
        public async Task<APIResponse> GetLecturesAndResourcesByAdmin(string searchString, string sortBy, int page = 1)
        {
            try
            {
                var user = await _context.Users.Include(role => role.UserRole)
                          .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
                if (!user.UserRole.LecturesAndResourcesSee)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "User does not have permission to view."
                    };
                }
                var allLecARes = _context.LecturesAndResourcesL
                    .Include(sb => sb.SubjectNavigation)
                        .ThenInclude(u => u.UserNavigation)
                        .ThenInclude(role => role.UserRole)
                        .AsQueryable();

                if (!string.IsNullOrEmpty(searchString))
                {
                    searchString = searchString.ToLower();
                    allLecARes = allLecARes.Where(las =>
                        las.SubjectNavigation.SubjectName.ToLower().Contains(searchString) ||
                        las.FileName.ToLower().Contains(searchString)
                    );
                }
                allLecARes = allLecARes.OrderBy(sb => sb.FileName);
                if (!string.IsNullOrEmpty(sortBy))
                {
                    switch (sortBy)
                    {
                        case "typeofdoc_asc":
                            allLecARes = allLecARes.OrderBy(s => s.TypeOfDocument);
                            break;
                        case "typeofdoc_desc":
                            allLecARes = allLecARes.OrderByDescending(s => s.TypeOfDocument);
                            break;
                        case "size_asc":
                            allLecARes = allLecARes.OrderBy(s => s.FileSize);
                            break;
                        case "size_desc":
                            allLecARes = allLecARes.OrderByDescending(s => s.FileSize);
                            break;
                        case "lsm_asc":
                            allLecARes = allLecARes.OrderBy(s => s.LastModifiedDate);
                            break;
                        case "lsm_desc":
                            allLecARes = allLecARes.OrderByDescending(s => s.LastModifiedDate);
                            break;
                    }
                }
                if (page < 1)
                {
                    return new APIResponse
                    {
                        Success = true,
                        Message = "Had found.",
                        Data = _mapper.Map<List<LecturesAndResourcesModelView>>(allLecARes)
                    };
                }
                var result = PaginatedList<LecturesAndResources>.Create(allLecARes, page, PAGE_SIZE);
                return new APIResponse
                {
                    Success = true,
                    Message = "Had found.",
                    Data = _mapper.Map<List<LecturesAndResourcesModelView>>(result)
                };
            }
            catch
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Not found.",
                };
            }
        }
        public async Task<APIResponse> GetLecturesFromInstructors(string subjectId, string searchString, string sortBy, int page = 1)
        {
            try
            {
                var subject = await _context.Subjects.FindAsync(Guid.Parse(subjectId));
                if (subject == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Subject not found."
                    };
                }
                var user = await _context.Users.Include(role => role.UserRole)
                          .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));

                var allLecARes = _context.LecturesAndResourcesL
                    .Include(sb => sb.SubjectNavigation)
                        .ThenInclude(u => u.UserNavigation)
                    .Where(s => s.SubjectNavigation.UserNavigation.UserId == user.UserId
                          && s.SubjectNavigation.SubjectId.Equals(Guid.Parse(subjectId))
                          && s.TypeOfDocument.Equals("Bài giảng")).AsQueryable();

                if (!string.IsNullOrEmpty(searchString))
                {
                    searchString = searchString.ToLower();
                    allLecARes = allLecARes.Where(las =>
                        las.SubjectNavigation.SubjectName.ToLower().Contains(searchString) ||
                        las.FileName.ToLower().Contains(searchString)
                    );
                }
                allLecARes = allLecARes.OrderBy(sb => sb.FileName);
                if (!string.IsNullOrEmpty(sortBy))
                {
                    switch (sortBy)
                    {
                        case "size_asc":
                            allLecARes = allLecARes.OrderBy(s => s.FileSize);
                            break;
                        case "size_desc":
                            allLecARes = allLecARes.OrderByDescending(s => s.FileSize);
                            break;
                        case "lsm_asc":
                            allLecARes = allLecARes.OrderBy(s => s.LastModifiedDate);
                            break;
                        case "lsm_desc":
                            allLecARes = allLecARes.OrderByDescending(s => s.LastModifiedDate);
                            break;
                    }
                }
                if (page < 1)
                {
                    return new APIResponse
                    {
                        Success = true,
                        Message = "Had found.",
                        Data = _mapper.Map<List<LecturesAndResourcesModelView>>(allLecARes)
                    };
                }
                var result = PaginatedList<LecturesAndResources>.Create(allLecARes, page, PAGE_SIZE);
                return new APIResponse
                {
                    Success = true,
                    Message = "Had found.",
                    Data = _mapper.Map<List<LecturesAndResourcesModelView>>(result)
                };
            }
            catch
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Not found.",
                };
            }
        }
        public async Task<APIResponse> GetResourcesFromInstructors(string subjectId, string searchString, string sortBy, int page = 1)
        {
            try
            {
                var subject = await _context.Subjects.FindAsync(Guid.Parse(subjectId));
                if (subject == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Subject not found."
                    };
                }
                var user = await _context.Users.Include(role => role.UserRole)
                          .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));

                var allLecARes = _context.LecturesAndResourcesL
                    .Include(sb => sb.SubjectNavigation)
                        .ThenInclude(u => u.UserNavigation)
                    .Where(s => s.SubjectNavigation.UserNavigation.UserId == user.UserId
                          && s.SubjectNavigation.SubjectId.Equals(Guid.Parse(subjectId))
                          && s.TypeOfDocument.Equals("Tài nguyên")).AsQueryable();

                if (!string.IsNullOrEmpty(searchString))
                {
                    searchString = searchString.ToLower();
                    allLecARes = allLecARes.Where(las =>
                        las.SubjectNavigation.SubjectName.ToLower().Contains(searchString) ||
                        las.FileName.ToLower().Contains(searchString)
                    );
                }
                allLecARes = allLecARes.OrderBy(sb => sb.FileName);
                if (!string.IsNullOrEmpty(sortBy))
                {
                    switch (sortBy)
                    {
                        case "size_asc":
                            allLecARes = allLecARes.OrderBy(s => s.FileSize);
                            break;
                        case "size_desc":
                            allLecARes = allLecARes.OrderByDescending(s => s.FileSize);
                            break;
                        case "lsm_asc":
                            allLecARes = allLecARes.OrderBy(s => s.LastModifiedDate);
                            break;
                        case "lsm_desc":
                            allLecARes = allLecARes.OrderByDescending(s => s.LastModifiedDate);
                            break;
                    }
                }
                if (page < 1)
                {
                    return new APIResponse
                    {
                        Success = true,
                        Message = "Had found.",
                        Data = _mapper.Map<List<LecturesAndResourcesModelView>>(allLecARes)
                    };
                }
                var result = PaginatedList<LecturesAndResources>.Create(allLecARes, page, PAGE_SIZE);
                return new APIResponse
                {
                    Success = true,
                    Message = "Had found.",
                    Data = _mapper.Map<List<LecturesAndResourcesModelView>>(result)
                };
            }
            catch
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Not found.",
                };
            }
        }
        #endregion
        //POST
        #region
        public async Task<APIResponse> UploadLectureFile(LecturesAndResourcesModelCreate model)
        {
            var subject = _context.Subjects.FindAsync(model.SubjectId);
            if (subject == null)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Subject not found"
                };
            }
            foreach (var file in model.Files)
            {
                // Upload file to Cloudinary
                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    Folder = "resources_of_lms_folder/lecture_folder"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                // Save file info to database
                var fileInfo = new LecturesAndResources
                {
                    SubjectId = model.SubjectId,
                    TypeOfDocument = "Bài giảng",
                    Status = "Chờ phê duyệt",
                    UploadDate = DateTime.Now,
                    //file info
                    FileType = Path.GetExtension(file.FileName),
                    FileName = file.FileName,
                    FileSize = $"{(double)file.Length / (1024 * 1024):0.##} MB",
                    FileUrl = uploadResult.SecureUri.ToString(),
                    FileViewUrl = (Path.GetExtension(file.FileName) == ".mp3" || Path.GetExtension(file.FileName) == ".mp4" || Path.GetExtension(file.FileName) == ".pdf")
                    ? uploadResult.SecureUri.ToString() : String.Concat("https://docs.google.com/viewer?url=", uploadResult.SecureUri.ToString())
                };

                _context.LecturesAndResourcesL.Add(fileInfo);
                await _context.SaveChangesAsync();

            }
            return new APIResponse
            {
                Success = true,
                Message = "Upload lecture file success."
            };

        }
        public async Task<APIResponse> UploadResourceFile(LecturesAndResourcesModelCreate model)
        {
            var subject = _context.Subjects.FindAsync(model.SubjectId);
            if (subject == null)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Subject not found"
                };
            }
            foreach (var file in model.Files)
            {
                // Upload file to Cloudinary
                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    Folder = "resources_of_lms_folder/resoucer_folder"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                // Save file info to database
                var fileInfo = new LecturesAndResources
                {
                    SubjectId = model.SubjectId,
                    TypeOfDocument = "Tài nguyên",
                    Status = "Chờ phê duyệt",
                    UploadDate = DateTime.Now,
                    //file info
                    FileType = Path.GetExtension(file.FileName),
                    FileName = file.FileName,
                    FileSize = $"{(double)file.Length / (1024 * 1024):0.##} MB",
                    FileUrl = uploadResult.SecureUri.ToString(),
                    FileViewUrl = (Path.GetExtension(file.FileName) == ".mp3" || Path.GetExtension(file.FileName) == ".mp4" || Path.GetExtension(file.FileName) == ".pdf")
                    ? uploadResult.SecureUri.ToString() : String.Concat("https://docs.google.com/viewer?url=", uploadResult.SecureUri.ToString())
                };

                _context.LecturesAndResourcesL.Add(fileInfo);
                await _context.SaveChangesAsync();

            }
            return new APIResponse
            {
                Success = true,
                Message = "Upload resoucer file success."
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
                if (!user.UserRole.LecturesAndResourcesEdit)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "User does not have permission to edit."
                    };
                }
                var fileFound = await _context.LecturesAndResourcesL.FindAsync(int.Parse(fileId));
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
                if (!user.UserRole.LecturesAndResourcesEdit)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "User does not have permission to edit."
                    };
                }
                var fileFound = await _context.LecturesAndResourcesL.FindAsync(int.Parse(fileId));
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
        public async Task<APIResponse> ApproveMultipleFile(List<string> fileIds)
        {
            try
            {
                var user = await _context.Users.Include(role => role.UserRole)
                          .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
                if (!user.UserRole.LecturesAndResourcesEdit)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "User does not have permission to edit."
                    };
                }
                foreach (var fileId in fileIds)
                {
                    var fileFound = await _context.LecturesAndResourcesL.FindAsync(int.Parse(fileId));
                    if (fileFound == null)
                    {
                        return new APIResponse
                        {
                            Success = false,
                            Message = $"File with ID {fileId} not found."
                        };
                    }

                    fileFound.Approve = true;
                    fileFound.Status = "Đã phê duyệt";
                    fileFound.Note = null;
                }
                await _context.SaveChangesAsync();
                return new APIResponse { Success = true, Message = "ApproveMultipleFile successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Error ApproveMultipleFile: {ex.Message}",
                };
            }
        }
        public async Task<APIResponse> NotApproveFile(string fileId, string note)
        {
            try
            {
                var user = await _context.Users.Include(role => role.UserRole)
                          .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
                if (!user.UserRole.LecturesAndResourcesEdit)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "User does not have permission to edit."
                    };
                }
                var fileFound = await _context.LecturesAndResourcesL.FindAsync(int.Parse(fileId));
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
        public async Task<APIResponse> NotApproveMultipleFile(List<string> fileIds, string note)
        {
            try
            {
                var user = await _context.Users.Include(role => role.UserRole)
                          .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
                if (!user.UserRole.LecturesAndResourcesEdit)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "User does not have permission to edit."
                    };
                }

                foreach (var fileId in fileIds)
                {
                    var fileFound = await _context.LecturesAndResourcesL.FindAsync(int.Parse(fileId));
                    if (fileFound == null)
                    {
                        return new APIResponse
                        {
                            Success = false,
                            Message = $"File with ID {fileId} not found."
                        };
                    }

                    fileFound.Approve = false;
                    fileFound.Status = "Không phê duyệt";
                    fileFound.Note = note;
                }

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
        public async Task<APIResponse> ChageLectureFile(string fileId, IFormFile newFile)
        {
            //Kiểm tra file có không
            #region
            var fileFound = await _context.LecturesAndResourcesL.FirstOrDefaultAsync(f => f.Id == int.Parse(fileId)
                            && f.TypeOfDocument.Equals("Bài giảng"));
            if (fileFound == null)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "File not found."
                };
            }
            #endregion
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
                    PublicIds = new List<string> { "resources_of_lms_folder/lecture_folder/" + fileName },
                    Type = "upload",
                    ResourceType = ResourceType.Raw
                };

                var deletionResult = _cloudinary.DeleteResources(deletionParams);
                // Kiểm tra xem hình ảnh đã được xóa thành công hay không
                if (deletionResult != null && deletionResult.Deleted != null && deletionResult.Deleted.Count > 0)
                {
                    // Xóa hình ảnh thành công
                    string fileUrl;
                    if (newFile != null)
                    {
                        // Tải lên ảnh lên Cloudinary
                        var uploadParams = new RawUploadParams()
                        {
                            File = new FileDescription(newFile.FileName, newFile.OpenReadStream()),
                            // Các tham số tải lên khác
                            Folder = "resources_of_lms_folder/lecture_folder"
                        };
                        var uploadResult = _cloudinary.Upload(uploadParams);

                        // Lấy URL của ảnh từ kết quả tải lên
                        fileUrl = uploadResult.SecureUrl.ToString();
                        if (fileUrl == null)
                        {
                            return new APIResponse
                            {
                                Success = false,
                                Message = "Upload file failed."
                            };
                        }
                        fileFound.Status = "Chờ phê duyệt";
                        fileFound.LastModifiedDate = DateTime.Now;
                        fileFound.Approve = null;
                        fileFound.Note = null;
                        //file info
                        fileFound.FileType = Path.GetExtension(newFile.FileName);
                        fileFound.FileName = newFile.FileName;
                        fileFound.FileSize = $"{(double)newFile.Length / (1024 * 1024):0.##} MB";
                        fileFound.FileUrl = uploadResult.SecureUri.ToString();
                        fileFound.FileViewUrl = (Path.GetExtension(newFile.FileName) == ".mp3" || Path.GetExtension(newFile.FileName) == ".mp4" || Path.GetExtension(newFile.FileName) == ".pdf")
                        ? uploadResult.SecureUri.ToString() : String.Concat("https://docs.google.com/viewer?url=", uploadResult.SecureUri.ToString());
                        await _context.SaveChangesAsync();
                        return new APIResponse { Success = true, Message = "ChageLectureFile successfully" };
                    }
                    else
                    {
                        return new APIResponse { Success = true, Message = "No file found" };
                    }

                }
                else
                {
                    return new APIResponse { Success = false, Message = "Deleting current file failed" };
                }
            }

            return new APIResponse { Success = true, Message = "Something Error" };
        }
        public async Task<APIResponse> ChageResourceFile(string fileId, IFormFile newFile)
        {
            //Kiểm tra file có không
            var fileFound = await _context.LecturesAndResourcesL.FirstOrDefaultAsync(f => f.Id == int.Parse(fileId)
                            && f.TypeOfDocument.Equals("Tài nguyên"));
            if (fileFound == null)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "File not found."
                };
            }
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
                    PublicIds = new List<string> { "resources_of_lms_folder/resoucer_folder/" + fileName },
                    Type = "upload",
                    ResourceType = ResourceType.Raw
                };

                var deletionResult = _cloudinary.DeleteResources(deletionParams);
                // Kiểm tra xem hình ảnh đã được xóa thành công hay không
                if (deletionResult != null && deletionResult.Deleted != null && deletionResult.Deleted.Count > 0)
                {
                    // Xóa hình ảnh thành công
                    string fileUrl;
                    if (newFile != null)
                    {
                        // Tải lên ảnh lên Cloudinary
                        var uploadParams = new RawUploadParams()
                        {
                            File = new FileDescription(newFile.FileName, newFile.OpenReadStream()),
                            // Các tham số tải lên khác
                            Folder = "resources_of_lms_folder/resoucer_folder"
                        };
                        var uploadResult = _cloudinary.Upload(uploadParams);

                        // Lấy URL của ảnh từ kết quả tải lên
                        fileUrl = uploadResult.SecureUrl.ToString();
                        if (fileUrl == null)
                        {
                            return new APIResponse
                            {
                                Success = false,
                                Message = "Upload file failed."
                            };
                        }
                        fileFound.Status = "Chờ phê duyệt";
                        fileFound.LastModifiedDate = DateTime.Now;
                        fileFound.Approve = null;
                        fileFound.Note = null;
                        //file info
                        fileFound.FileType = Path.GetExtension(newFile.FileName);
                        fileFound.FileName = newFile.FileName;
                        fileFound.FileSize = $"{(double)newFile.Length / (1024 * 1024):0.##} MB";
                        fileFound.FileUrl = uploadResult.SecureUri.ToString();
                        fileFound.FileViewUrl = (Path.GetExtension(newFile.FileName) == ".mp3" || Path.GetExtension(newFile.FileName) == ".mp4" || Path.GetExtension(newFile.FileName) == ".pdf")
                        ? uploadResult.SecureUri.ToString() : String.Concat("https://docs.google.com/viewer?url=", uploadResult.SecureUri.ToString());
                        await _context.SaveChangesAsync();
                        return new APIResponse { Success = true, Message = "ChageResourceFile successfully" };
                    }
                    else
                    {
                        return new APIResponse { Success = true, Message = "No file found" };
                    }

                }
                else
                {
                    return new APIResponse { Success = false, Message = "Deleting current file failed" };
                }
            }

            return new APIResponse { Success = true, Message = "Something Error" };
        }     
        #endregion
        //DELETE
        #region
        #endregion
    }
}
