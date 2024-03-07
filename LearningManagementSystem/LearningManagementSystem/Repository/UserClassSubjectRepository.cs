using AutoMapper;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.PaginatedList;
using LearningManagementSystem.Models.SubjectModel;
using LearningManagementSystem.Models.UserClassSubjectModel;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LearningManagementSystem.Repository
{
    public class UserClassSubjectRepository : InterfaceUserClassSubjectRepository
    {
        private readonly LearningManagementSystemContext _context;
        private readonly IMapper _mapper;
        private static int PAGE_SIZE { get; set; } = 5;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserClassSubjectRepository(LearningManagementSystemContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._mapper = mapper;
            this._httpContextAccessor = httpContextAccessor;
        }
        //GET
        #region
        public async Task<APIResponse> GetSubjectStudent(string academicYear, string semester, string searchString, string sortBy, bool? mark, int page = 1)
        {
            try
            {
                var user = await _context.Users.Include(role => role.UserRole)
                          .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));

                var allSubjectStudent = _context.UserClassSubjects
                    .Include(ur => ur.UserNavigation)
                    .Include(c => c.ClassNavigation)
                        .ThenInclude(fc => fc.FacultyNavigation)
                    .Include(sb => sb.SubjectNavigation)
                        .ThenInclude(u => u.UserNavigation)
                    .Include(sb => sb.SubjectNavigation)
                        .ThenInclude(osi => osi.OtherSubjectInformations)
                    .Include(sb => sb.SubjectNavigation)
                        .ThenInclude(osi => osi.SubjectTopics)
                    .Where(st => st.UserNavigation.UserId == user.UserId)
                    .Where(c => c.ClassNavigation.AcademicYear.Equals(academicYear) && c.ClassNavigation.Semester.Equals(semester))
                    .AsQueryable();

                if (!string.IsNullOrEmpty(searchString))
                {
                    searchString = searchString.ToLower();
                    allSubjectStudent = allSubjectStudent.Where(ucs =>
                        ucs.SubjectNavigation.SubjectCode.ToLower().Contains(searchString) ||
                        ucs.SubjectNavigation.SubjectName.ToLower().Contains(searchString) ||
                        string.Concat(ucs.SubjectNavigation.UserNavigation.FirstName, " ",
                        ucs.SubjectNavigation.UserNavigation.LastName).ToLower().Contains(searchString));
                }
                allSubjectStudent = allSubjectStudent.OrderBy(sb => sb.SubjectNavigation.SubjectName);
                if (!string.IsNullOrEmpty(sortBy))
                {
                    switch (sortBy)
                    {
                        case "sname_asc":
                            allSubjectStudent = allSubjectStudent.OrderBy(sb => sb.SubjectNavigation.SubjectName);
                            break;
                        case "sname_desc":
                            allSubjectStudent = allSubjectStudent.OrderByDescending(sb => sb.SubjectNavigation.SubjectName);
                            break;
                        case "lscode_asc":
                            allSubjectStudent = allSubjectStudent.OrderBy(s => s.LastRecent);
                            break;
                        case "lscode_desc":
                            allSubjectStudent = allSubjectStudent.OrderByDescending(s => s.LastRecent);
                            break;
                    }
                }
                if(mark != null)
                {
                    if ((bool)mark)
                    {
                        allSubjectStudent = allSubjectStudent.Where(ucs => ucs.Mark);
                    }
                    else
                    {
                        allSubjectStudent = allSubjectStudent.Where(ucs => !ucs.Mark);
                    }
                }
                if (page < 1)
                {
                    return new APIResponse
                    {
                        Success = true,
                        Message = "Had found.",
                        Data = _mapper.Map<List<UserClassSubjectModelView>>(allSubjectStudent)
                    };
                }
                var result = PaginatedList<UserClassSubject>.Create(allSubjectStudent, page, PAGE_SIZE);
                return new APIResponse
                {
                    Success = true,
                    Message = "Had found.",
                    Data = _mapper.Map<List<UserClassSubjectModelView>>(result)
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
        public async Task<APIResponse> CreatorNew(UserClassSubjectModelCreate model)
        {
            try
            {
                // Kiểm tra xem ClassId có tồn tại hay không
                var existingClass = await _context.Classs.FirstOrDefaultAsync(c => c.ClassId == model.ClassId);
                if (existingClass == null)
                {
                    return new APIResponse { Success = false, Message = "ClassId Invalid." };
                }

                // Kiểm tra xem SubjectId có tồn tại hay không
                var existingSubject = await _context.Subjects.FirstOrDefaultAsync(s => s.SubjectId == model.SubjectId);
                if (existingSubject == null)
                {
                    return new APIResponse { Success = false, Message = "SubjectId Invalid." };
                }

                // Kiểm tra từng UserId trong danh sách UserId có tồn tại hay không
                foreach (var userId in model.UserId)
                {
                    var existingUser = await _context.Users
                        .Include(u => u.UserRole)
                        .FirstOrDefaultAsync(u => u.UserId == userId && u.UserRole.RoleName.Equals("Học viên"));
                    if (existingUser == null)
                    {
                        return new APIResponse { Success = false, Message = $"UserId {userId} Invalid or User not a Student." };
                    }
                    _context.UserClassSubjects.Add(new UserClassSubject
                    {
                        UserId = existingUser.UserId,
                        ClassId = existingClass.ClassId,
                        SubjectId = existingSubject.SubjectId
                    });
                }
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
                return new APIResponse { Success = true, Message = "UserClassSubject created successfully." };

            }
            catch (Exception ex)
            {
                return new APIResponse { Success = false, Message = $"Error: {ex.Message}" };
            }
        }
        #endregion
        //PUT
        #region
        public async Task<APIResponse> UpdateLastRecent(string subjectId, string classId)
        {
            try
            {
                // Lấy người dùng từ email của người dùng hiện tại
                var user = await _context.Users.Include(role => role.UserRole)
                    .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
                // Kiểm tra xem người dùng có đang tham gia vào môn học có SubjectId không
                var ucs = await _context.UserClassSubjects
                    .Include(u => u.UserNavigation)
                    .Include(c => c.ClassNavigation)
                    .Include(s => s.SubjectNavigation)
                    .FirstOrDefaultAsync(ucs => ucs.SubjectId == Guid.Parse(subjectId)
                    && ucs.UserNavigation.UserId.Equals(user.UserId)
                    && ucs.ClassNavigation.ClassId.Equals(Guid.Parse(classId))
                    && ucs.SubjectNavigation.SubjectId.Equals(Guid.Parse(subjectId)));

                if (ucs == null)
                {
                    return new APIResponse { Success = false, Message = "Student-Class-Subject not found." };
                }

                // Cập nhật LastRecent cho người dùng
                ucs.LastRecent = DateTime.UtcNow;

                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();

                return new APIResponse { Success = true, Message = "LastRecent updated successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { Success = false, Message = $"Error updating LastRecent: {ex.Message}" };
            }
        }
        public async Task<APIResponse> UpdateMark(string subjectId, string classId)
        {
            try
            {
                // Lấy người dùng từ email của người dùng hiện tại
                var user = await _context.Users.Include(role => role.UserRole)
                    .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
                // Kiểm tra xem người dùng có đang tham gia vào môn học có SubjectId không
                var ucs = await _context.UserClassSubjects
                    .Include(u => u.UserNavigation)
                    .Include(c => c.ClassNavigation)
                    .Include(s => s.SubjectNavigation)
                    .FirstOrDefaultAsync(ucs => ucs.SubjectId == Guid.Parse(subjectId)
                    && ucs.UserNavigation.UserId.Equals(user.UserId)
                    && ucs.ClassNavigation.ClassId.Equals(Guid.Parse(classId))
                    && ucs.SubjectNavigation.SubjectId.Equals(Guid.Parse(subjectId)));

                if (ucs == null)
                {
                    return new APIResponse { Success = false, Message = "Student-Class-Subject not found." };
                }

                // Cập nhật LastRecent cho người dùng
                if (ucs.Mark)
                {
                    ucs.Mark = false;
                }
                else
                {
                    ucs.Mark = true;
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();

                return new APIResponse { Success = true, Message = "Mark updated successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { Success = false, Message = $"Error updating Mark: {ex.Message}" };
            }
        }
        #endregion
    }
}
