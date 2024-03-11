using AutoMapper;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.FacultyModel;
using LearningManagementSystem.Models.PaginatedList;
using LearningManagementSystem.Models.SubjectModel;
using LearningManagementSystem.Models.UserRoleModels;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Security.Claims;

namespace LearningManagementSystem.Repository
{
    public class SubjectRepository : InterfaceSubjectRepository
    {
        private readonly LearningManagementSystemContext _context;
        private readonly IMapper _mapper;
        private static int PAGE_SIZE { get; set; } = 5;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public SubjectRepository(LearningManagementSystemContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._mapper = mapper;
            this._httpContextAccessor = httpContextAccessor;
        }
        //GET
        #region
        public async Task<List<SubjectModelView>> GetAll(string searchString, int page = 1)
        {
            var allSunject = _context.Subjects
                .Include(ur => ur.UserNavigation)
                    .ThenInclude(ur => ur.UserRole)
                .Include(s => s.OtherSubjectInformations)
                .Include(s => s.SubjectTopics)
                    .ThenInclude(lr => lr.Lessons)
                .AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                allSunject = allSunject.Where(f =>
                    f.SubjectCode.ToLower().Contains(searchString) ||
                    f.SubjectName.ToLower().Contains(searchString)
                );
            }
            allSunject = allSunject.OrderBy(sb => sb.SubjectName);
            if (page < 1)
            {
                return _mapper.Map<List<SubjectModelView>>(allSunject);
            }
            var result = PaginatedList<Subject>.Create(allSunject, page, PAGE_SIZE);
            return _mapper.Map<List<SubjectModelView>>(result);
        }

        public async Task<APIResponse> GetById(string id)
        {
            try
            {
                return new APIResponse
                {
                    Success = true,
                    Message = "Had found.",
                    Data = _mapper.Map<SubjectModelView>(await _context.Subjects
                    .Include(ur => ur.UserNavigation)
                        .ThenInclude(ur => ur.UserRole)
                    .Include(s => s.OtherSubjectInformations)
                    .Include(s => s.SubjectTopics)
                        .ThenInclude(lr => lr.Lessons)
                    .FirstOrDefaultAsync(s => s.SubjectId == Guid.Parse(id)))
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
        public async Task<APIResponse> GetSubjectByUserId(string searchString, string sortBy, int page = 1)
        {
            try
            {
                var user = await _context.Users.Include(role => role.UserRole)
                          .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));

                var allSubject = _context.Subjects
                    .Include(ur => ur.UserNavigation)
                        .ThenInclude(ur => ur.UserRole)
                    .Include(s => s.OtherSubjectInformations)
                    .Include(s => s.SubjectTopics)
                        .ThenInclude(lr => lr.Lessons)
                    .Where(s => s.UserNavigation.UserId == user.UserId).AsQueryable();

                if (!string.IsNullOrEmpty(searchString))
                {
                    searchString = searchString.ToLower();
                    allSubject = allSubject.Where(f =>
                        f.SubjectCode.ToLower().Contains(searchString) ||
                        f.SubjectName.ToLower().Contains(searchString)
                    );
                }
                allSubject = allSubject.OrderBy(sb => sb.SubjectName);
                if (!string.IsNullOrEmpty(sortBy))
                {
                    switch (sortBy)
                    {
                        case "sname_asc":
                            allSubject = allSubject.OrderBy(s => s.SubjectName);
                            break;
                        case "sname_desc":
                            allSubject = allSubject.OrderByDescending(s => s.SubjectName);
                            break;
                        case "lscode_asc":
                            allSubject = allSubject.OrderBy(s => s.LastRecent);
                            break;
                        case "lscode_desc":
                            allSubject = allSubject.OrderByDescending(s => s.LastRecent);
                            break;
                    }
                }
                if (page < 1)
                {
                    return new APIResponse
                    {
                        Success = true,
                        Message = "Had found.",
                        Data = _mapper.Map<List<SubjectModelView>>(allSubject)
                    };
                }
                var result = PaginatedList<Subject>.Create(allSubject, page, PAGE_SIZE);
                return new APIResponse
                {
                    Success = true,
                    Message = "Had found.",
                    Data = _mapper.Map<List<SubjectModelView>>(result)
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
        public async Task<APIResponse> CreateNew(SubjectModelCreate model)
        {
            var lecturer = await _context.Users
                .Include(r => r.UserRole)
                .Where(ur => ur.UserId == model.LecturerId && ur.UserRole.RoleName.Equals("Giảng viên"))
                .FirstOrDefaultAsync();
            if (lecturer == null)
            {
                return new APIResponse
                {
                    Success = true,
                    Message = "Users are not instructors .",
                };
            }
            var newSubject = _mapper.Map<Subject>(model);
            newSubject.SubjectId = Guid.NewGuid();
            await _context.AddAsync(newSubject);
            await _context.SaveChangesAsync();
            return new APIResponse
            {
                Success = true,
                Message = "Created successfully.",
                Data = _mapper.Map<SubjectModelView>(newSubject)
            };
        }
        #endregion
        //PUT
        #region
        public async Task<APIResponse> UpdateLastRecentBySubjectId(string subjectId)
        {
            try
            {
                // Lấy người dùng từ email của người dùng hiện tại
                var user = await _context.Users.Include(role => role.UserRole)
                    .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
                // Kiểm tra xem người dùng có đang tham gia vào môn học có SubjectId không
                var subject = await _context.Subjects
                    .Include(s => s.UserNavigation)
                    .FirstOrDefaultAsync(s => s.SubjectId == Guid.Parse(subjectId)
                    && s.UserNavigation.UserId.Equals(user.UserId));
                if (subject == null)
                {
                    return new APIResponse { Success = false, Message = "Subject-Teacher not found." };
                }

                // Cập nhật LastRecent cho người dùng
                subject.LastRecent = DateTime.UtcNow;

                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();

                return new APIResponse { Success = true, Message = "LastRecent updated successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { Success = false, Message = $"Error updating LastRecent: {ex.Message}" };
            }
        }

        public async Task<APIResponse> UpdateById(string id, SubjectModelUpdate model)
        {
            try
            {
                var subject = await _context.Subjects.FindAsync(Guid.Parse(id));

                if (subject == null)
                {
                    return new APIResponse { Success = false, Message = "Subject not found" };
                }
                // Cập nhật từng trường một từ model vào userRole
                #region
                foreach (var property in typeof(SubjectModelUpdate).GetProperties())
                {
                    var modelValue = property.GetValue(model);
                    if (modelValue != null)
                    {
                        var subjectProperty = typeof(Subject).GetProperty(property.Name);
                        if (subjectProperty != null)
                        {
                            subjectProperty.SetValue(subject, modelValue);
                        }
                    }
                }
                #endregion
                await _context.SaveChangesAsync();

                return new APIResponse { Success = true, Message = "Subject updated successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse { Success = false, Message = $"Error updating Subject: {ex.Message}" };
            }
        }
        #endregion
    }
}
