using AutoMapper;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.UserNotificationsModel;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Repository
{
    public class UNRepository : InterfaceUNRepository
    {
        private readonly LearningManagementSystemContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static int PAGE_SIZE { get; set; } = 5;
        public UNRepository(LearningManagementSystemContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        //GET
        #region
        public async Task<APIResponse> GetAll()
        {
            var getAllUN = await _context.UserNotificationss
                .Include(u => u.UserNotificationsNavigation)
                .Include(QaAF => QaAF.QaAFollowersNavigation)
                    .ThenInclude(QaA => QaA.QuestionAndAnswerNavigation)
                        .ThenInclude(c => c.ClassNavigation)
                .Include(QaAF => QaAF.QaAFollowersNavigation)
                    .ThenInclude(QaA => QaA.QuestionAndAnswerNavigation)
                        .ThenInclude(c => c.LessonNavigation)
                            .ThenInclude(st => st.SubjectTopicNavigation)
                                .ThenInclude(s => s.SubjectIdNavigation)
                .Include(sa => sa.SubjectAnnouncementNavigation)
                    .ThenInclude(sa => sa.SubjectAnnouncementNavigation)
                .Include(sa => sa.SubjectAnnouncementNavigation)
                    .ThenInclude(sa => sa.ClassAnnouncementNavigation)
                .Include(QaA => QaA.QuestionAndAnswerNavigation)
                    .ThenInclude(c => c.ClassNavigation)
                .Include(QaA => QaA.QuestionAndAnswerNavigation)
                    .ThenInclude(c => c.LessonNavigation)
                        .ThenInclude(st => st.SubjectTopicNavigation)
                            .ThenInclude(s => s.SubjectIdNavigation)
                .ToListAsync();
            return new APIResponse
            {
                Success = true,
                Message = "All USER Notifications",
                Data = _mapper.Map<List<UserNotificationViewModel>>(getAllUN)
            };
        }
        #endregion
        //POST
        #region
        public async Task CreateUN(UserNotificationModelCreate model)
        {
            var newUN = _mapper.Map<UserNotifications>(model);
            newUN.CreatedDate = DateTime.Now;
            await _context.AddAsync(newUN);
            await _context.SaveChangesAsync();
        }      
        #endregion
        //PUT
    }
}
