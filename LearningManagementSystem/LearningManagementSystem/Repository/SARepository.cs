using AutoMapper;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.PaginatedList;
using LearningManagementSystem.Models.QaAModel;
using LearningManagementSystem.Models.SAModel;
using LearningManagementSystem.Models.UserNotificationsModel;
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
        private readonly InterfaceUNRepository _interfaceUNRepository;

        public SARepository(LearningManagementSystemContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, InterfaceUNRepository interfaceUNRepository)
        {
            this._context = context;
            this._mapper = mapper;
            this._httpContextAccessor = httpContextAccessor;
            this._interfaceUNRepository = interfaceUNRepository;
        }
        //GET
        #region
        public async Task<APIResponse> GetSA(string? subjectId, string? classId, bool isNotice, int page = 1)
        {
            var subjectCheck = await _context.Subjects.FindAsync(Guid.Parse(subjectId));
            if (subjectId == null)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "SubjectId not found."
                };
            }
            
            var SA = _context.SubjectAnnouncements
                .Include(u => u.UserAnnouncementNavigation)
                .Include(c => c.ClassAnnouncementNavigation)
                .Include(s => s.SubjectAnnouncementNavigation)
                .Where(s => s.SubjectIdAnnouncement.Equals(Guid.Parse(subjectId)))
                .AsQueryable();

            if (!string.IsNullOrEmpty(classId))
            {
                var classCheck = await _context.Classs.FindAsync(Guid.Parse(classId));
                if (classCheck == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "ClassId not found."
                    };
                }
                SA = SA.Where(c => c.ClassIdAnnouncement.Equals(Guid.Parse(classId)));
            }
            if ((bool)isNotice)//True là thông báo
            {
                SA = SA.Where(c => c.SAInOtherSA == null && c.SAReplySA == null);
            }
            if (!(bool)isNotice)//False là các phần trả lời
            {
                SA = SA.Where(c => c.SAInOtherSA != null && c.SAReplySA != null);
            }
            if (page < 1)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Had found.",
                    Data = _mapper.Map<List<SubjectAnnouncementModelView>>(SA)
                };

            }
            var result = PaginatedList<SubjectAnnouncement>.Create(SA, page, PAGE_SIZE);
            return new APIResponse
            {
                Success = false,
                Message = "Had found.",
                Data = _mapper.Map<List<SubjectAnnouncementModelView>>(result)
            };
        }
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

            //var newSA = new List<SubjectAnnouncement>();
            foreach (var classObj in model.ListClassId)
            {
                var sa = _mapper.Map<SubjectAnnouncement>(model);
                sa.SACreatedDate = DateTime.Now;
                sa.UserIdAnnouncement = user.UserId;
                sa.ClassIdAnnouncement = classObj;
                //newSA.Add(sa);
                await _context.AddAsync(sa);
                await _context.SaveChangesAsync();
                var userInClass = await _context.UserClassSubjects
                    .Where(ucs => ucs.ClassId.Equals(classObj))
                    .ToListAsync();
                foreach (var userObj in userInClass)
                {
                    var newUN = new UserNotificationModelCreate
                    {
                        UserNotificationsContent = "Giảng viên " + $"{user.FirstName + " " + user.LastName} đã tạo thông báo môn học {subject.SubjectName}",
                        UserIdNotifications = (Guid)userObj.UserId,
                        SubjectAnnouncementId = sa.SubjectAnnouncementId
                    };
                    await _interfaceUNRepository.CreateUN(newUN);
                }
            }
            // Thêm danh sách bài giảng mới vào CSDL
            //await _context.SubjectAnnouncements.AddRangeAsync(newSA);
            //await _context.SaveChangesAsync();
            return new APIResponse
            {
                Success = true,
                Message = "CreateSA success."
            };

        }

        public async Task<APIResponse> CreateSA(SubjectAnnouncementModelCreateSingle model)
        {
            var classCheck = await _context.Classs.FindAsync(model.ClassIdAnnouncement);
            var subjectCheck = await _context.Subjects.FindAsync(model.SubjectIdAnnouncement);
            if (classCheck == null || subjectCheck == null)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "ClassId or SubjectId not found."
                };
            }
            var user = await _context.Users.Include(role => role.UserRole)
               .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
            var newSA = _mapper.Map<SubjectAnnouncement>(model);
            newSA.UserIdAnnouncement = user.UserId;
            newSA.SACreatedDate = DateTime.Now;
            await _context.AddAsync(newSA);
            await _context.SaveChangesAsync();
            return new APIResponse
            {
                Success = false,
                Message = "CreateSA success."
            };
        }
        #endregion
        //PUT
        #region
        #endregion
    }
}
