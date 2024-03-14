using AutoMapper;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.SAModel;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LearningManagementSystem.Repository
{
    public class SARepository : InterfaceSARepository
    {
        private readonly LearningManagementSystemContext _context;
        private readonly IMapper _mapper;
        private static int PAGE_SIZE { get; set; } = 5;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SARepository(LearningManagementSystemContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._mapper = mapper;
            this._httpContextAccessor = httpContextAccessor;
        }
        //GET
        #region
        #endregion
        //POST
        #region
        public async Task<APIResponse> CreateSA(SubjectAnnouncementModelCreate model)
        {

            var nonExistingClassIds = model.ListClassId.Where(classId =>
                !_context.Classs.Any(c => c.ClassId == classId)).ToList();

            if (nonExistingClassIds.Any())
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"The following ClassId does not exist in the database: {string.Join(", ", nonExistingClassIds)}"
                };
            }
            var subject = await _context.Subjects.FindAsync(model.SubjectIdAnnouncement);
            if (subject == null)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "SubjectId not found."
                };
            }

            var user = await _context.Users.Include(role => role.UserRole)
                          .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));

            var newSA = new List<SubjectAnnouncement>();
            foreach (var classObj in model.ListClassId)
            {
                //var sa = new SubjectAnnouncement
                //{
                //    SubjectAnnouncementTitle = model.SubjectAnnouncementTitle,
                //    SubjectAnnouncementContent = model.SubjectAnnouncementContent,
                //    SACreatedDate = DateTime.Now,
                //    UserIdAnnouncement = user.UserId,
                //    ClassIdAnnouncement = classObj,
                //    SubjectIdAnnouncement = model.SubjectIdAnnouncement

                //};
                var sa = _mapper.Map<SubjectAnnouncement>(model);
                sa.SACreatedDate = DateTime.Now;
                sa.UserIdAnnouncement = user.UserId;
                sa.ClassIdAnnouncement = classObj;
                newSA.Add(sa);
            }
            // Thêm danh sách bài giảng mới vào CSDL
            await _context.SubjectAnnouncements.AddRangeAsync(newSA);
            await _context.SaveChangesAsync();

            return new APIResponse
            {
                Success = true,
                Message = "CreateSA success."
            };

        }
        #endregion
        //PUT
        #region
        #endregion
    }
}
