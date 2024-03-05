using AutoMapper;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.UserBelongToFacultyModel;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Repository
{
    public class UserBelongToFacultyRepository : InterfaceUserBelongToFacultyRepository
    {
        private readonly LearningManagementSystemContext _context;
        private readonly IMapper _mapper;
        public UserBelongToFacultyRepository(LearningManagementSystemContext context, IMapper mapper) { 
            this._context = context;
            this._mapper = mapper;
        }

        //GET
        #region
        private async Task<int> CountUserIsStudentByFacultyId(string id)
        {
            var countTeacher = await _context.UserBelongToFacultys.Include(u => u.User).Where(ubf 
                => ubf.FacultyId.Equals(Guid.Parse(id))
                && ubf.User.UserRole.RoleName.Equals("Học viên")).CountAsync();
            return countTeacher;
        }

        private async Task<int> CountUserIsTeacherByFacultyId(string id)
        {
            var countTeacher = await _context.UserBelongToFacultys.Include(u => u.User).Where(ubf
                => ubf.FacultyId.Equals(Guid.Parse(id))
                && ubf.User.UserRole.RoleName.Equals("Giảng viên")).CountAsync();
            return countTeacher;
        }
        #endregion
        //POST
        #region
        public async Task<APIResponse> CreatorUserBelongToFaculty(UserBelongToFacultyModelCreate model)
        {
            try
            {
                // Lấy danh sách User từ UserId
                var users = await _context.Users
                    .Where(u => model.UserId.Contains(u.UserId))
                    .ToListAsync();

                // Kiểm tra FacultyId có tồn tại trong bảng Faculty không
                var faculty = await _context.Facultys.FindAsync(model.FacultyId);
                if (faculty == null)
                {
                    return new APIResponse { Success = false, Message = "Faculty not found." };
                }

                // Tạo các bản ghi mới trong bảng UserBelongToFaculty
                foreach (var user in users)
                {
                    _context.UserBelongToFacultys.Add(new UserBelongToFaculty
                    {
                        FacultyId = model.FacultyId,
                        UserId = user.UserId
                    });
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
                // Cập nhật số lượng học viên và số lượng giảng viên trong bảng Faculty
                faculty.NumberOfStudents = await CountUserIsStudentByFacultyId(model.FacultyId.ToString());
                faculty.NumberOfTeacher = await CountUserIsTeacherByFacultyId(model.FacultyId.ToString());

                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
                return new APIResponse { Success = true, Message = "Users added to Faculty successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { Success = false, Message = $"Error: {ex.Message}" };
            }
        }
        #endregion
        //PUT
        #region
        public async Task<APIResponse> SetHeadOfDepartmentByUserIdAndFacultyId(string userId, string facultyId)
        {
            try
            {
                // Kiểm tra userId không phải là học viên
                var user = await _context.Users
                    .Include(u => u.UserRole)
                    .FirstOrDefaultAsync(u => u.UserId.Equals(Guid.Parse(userId)) && !u.UserRole.RoleName.Equals("Học viên"));

                if (user == null)
                {
                    return new APIResponse { Success = false, Message = "User is not a teacher." };
                }

                // Kiểm tra facultyId có tồn tại trong bảng Faculty không
                var faculty = await _context.Facultys.FindAsync(Guid.Parse(facultyId));
                if (faculty == null)
                {
                    return new APIResponse { Success = false, Message = "Faculty not found." };
                }

                // Cập nhật IsHeadOfDepartment của bảng UserBelongToFaculty
                var userBelongToFaculty = await _context.UserBelongToFacultys
                    .FirstOrDefaultAsync(ubf => ubf.UserId.Equals(Guid.Parse(userId)) && ubf.FacultyId.Equals(Guid.Parse(facultyId)));

                if (userBelongToFaculty == null)
                {
                    return new APIResponse { Success = false, Message = "User does not belong to this faculty." };
                }

                userBelongToFaculty.IsHeadOfDepartment = true;

                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();

                return new APIResponse { Success = true, Message = "User set as Head of Department successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { Success = false, Message = $"Error: {ex.Message}" };
            }
        }
        #endregion
    }
}
