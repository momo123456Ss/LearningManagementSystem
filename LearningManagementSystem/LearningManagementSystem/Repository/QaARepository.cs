using AutoMapper;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.PaginatedList;
using LearningManagementSystem.Models.QaAModel;
using LearningManagementSystem.Models.SubjectModel;
using LearningManagementSystem.Models.UserNotificationsModel;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace LearningManagementSystem.Repository
{
    public class QaARepository : InterfaceQaARepository
    {
        private readonly LearningManagementSystemContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static int PAGE_SIZE { get; set; } = 5;
        private readonly InterfaceUNRepository _interfaceUNRepository;

        public QaARepository(LearningManagementSystemContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, InterfaceUNRepository interfaceUNRepository)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
            this._mapper = mapper;
            this._interfaceUNRepository = interfaceUNRepository;
        }
        //GET
        #region
        public async Task<List<QaAModelUserView>> GetQaA(string? subjectId, string? classId, int? subjectTopicId, int? lessonId, string? sortByCreatedDate, bool QaAIsFollow, bool myQuestions, bool QuestionsNoAnswer, int page = 1)
        {
            var user = await _context.Users.Include(role => role.UserRole)
               .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
            var QaA = _context.QuestionAndAnswers
                .Include(u => u.UserNavigation)
                .Include(f => f.QaAFollowerss)
                .Include(c => c.ClassNavigation)
                .Include(l => l.LessonNavigation)
                    .ThenInclude(st => st.SubjectTopicNavigation)
                        .ThenInclude(s => s.SubjectIdNavigation)
                .Where(qaa => qaa.LessonNavigation.SubjectTopicNavigation.SubjectId.Equals(Guid.Parse(subjectId)))
                .AsQueryable();
            QaA = QaA.OrderBy(qaa => qaa.QuestionAndAnswerId);
            if (subjectTopicId != null)
            {
                QaA = QaA.Where(qaa => qaa.LessonNavigation.SubjectTopicId == subjectTopicId);
            }
            if (lessonId != null)
            {
                QaA = QaA.Where(qaa => qaa.LessonIdComment == lessonId &&
                                    qaa.QaAInOtherQaA == null &&
                                    qaa.QaAReplyQaA == null); //QaAInOtherQaA và QaAReplyQaA = null thì đây là câu hỏi
            }
            if (!string.IsNullOrEmpty(classId))
            {
                QaA = QaA.Where(qaa => qaa.ClassIdComment.Equals(Guid.Parse(classId)));
            }
            if (!string.IsNullOrEmpty(sortByCreatedDate))
            {
                switch (sortByCreatedDate)
                {
                    case "created_asc":
                        QaA = QaA.OrderBy(qaa => qaa.QaACreatedDate);
                        break;
                    case "created_desc":
                        QaA = QaA.OrderByDescending(qaa => qaa.QaACreatedDate);
                        break;
                }
            }
            if (QaAIsFollow)
            {
                ICollection<QaAFollowers> isFollow = await _context.QaAFollowerss
                    .Include(f => f.QuestionAndAnswerNavigation)
                    .Where(f => f.UserIdFollower.Equals(user.UserId) &&
                    f.QuestionAndAnswerNavigation.QaAInOtherQaA == null &&
                    f.QuestionAndAnswerNavigation.QaAReplyQaA == null)
                    .ToListAsync();
                //List<int> QaAID = new List<int>();
                //foreach (var qaa in isFollow)
                //{
                //    QaAID.Add(qaa.QaAIdFollow);
                //}
                QaA = QaA.Where(qaa => qaa.QaAFollowerss.Any(follower => isFollow.Contains(follower)));
                //QaA = QaA.Where(qaa => QaAID.Contains(qaa.QuestionAndAnswerId));
                //QaA = QaA.Where(qaa => isFollow.Any(f => f.QaAIdFollow == qaa.QuestionAndAnswerId));

            }
            if ((bool)myQuestions)
            {
                QaA = QaA.Where(qaa => qaa.UserIdComment.Equals(user.UserId) &&
                                    qaa.QaAInOtherQaA == null && qaa.QaAReplyQaA == null);
            }

            if ((bool)QuestionsNoAnswer)
            {
                QaA = QaA.Where(qaa => qaa.NumberOfResponses == 0 &&
                                    qaa.QaAInOtherQaA == null && qaa.QaAReplyQaA == null);
            }

            if (page < 1)
            {
                return _mapper.Map<List<QaAModelUserView>>(QaA);
            }
            var result = PaginatedList<QuestionAndAnswer>.Create(QaA, page, PAGE_SIZE);
            return _mapper.Map<List<QaAModelUserView>>(result);
        }
        #endregion
        //POST
        #region
        public async Task<APIResponse> CreateQaA(QaAModelCreate model)
        {
            var lesson = await _context.Lessons.FindAsync(model.LessonIdComment);
            if (lesson == null)
            {
                return new APIResponse { Success = false, Message = "LessonId not found." };
            }
            var classId = await _context.Classs.FindAsync(model.ClassIdComment);
            if (lesson == null)
            {
                return new APIResponse { Success = false, Message = "ClassId not found." };
            }
            var user = await _context.Users.Include(role => role.UserRole)
               .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));

            var newComment = _mapper.Map<QuestionAndAnswer>(model);
            newComment.UserIdComment = user.UserId;
            newComment.QaACreatedDate = DateTime.Now;
            await _context.AddAsync(newComment);
            await _context.SaveChangesAsync();

            if (model.QaAInOtherQaA != null && model.QaAReplyQaA != null)
            {
                var comment = await _context.QuestionAndAnswers.FindAsync(model.QaAInOtherQaA);
                comment.NumberOfResponses += 1;
                await _context.SaveChangesAsync();
                var userReceivesNotification = await _context.QuestionAndAnswers.FirstOrDefaultAsync(x => x.QuestionAndAnswerId == model.QaAReplyQaA);
                var newUN = new UserNotificationModelCreate
                {
                    UserNotificationsContent = $"{user.FirstName + " " + user.LastName} đã phản hồi bình luận của bạn",
                    UserIdNotifications = userReceivesNotification.UserIdComment,
                    QuestionAndAnswerId = model.QaAReplyQaA
                };

                await _interfaceUNRepository.CreateUN(newUN);
            }


            return new APIResponse
            {
                Success = true,
                Message = "Created successfully.",
                Data = _mapper.Map<QaAModelView>(newComment)
            };
        }
        public async Task<APIResponse> QaAFollow(int QaAId)
        {
            var qaa = await _context.QuestionAndAnswers.FindAsync(QaAId);
            if (qaa == null)
            {
                return new APIResponse { Success = false, Message = "QaA not found." };
            }
           
            var user = await _context.Users.Include(role => role.UserRole)
               .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));

            var checkFollow = await _context.QaAFollowerss.FirstOrDefaultAsync(
                cf => cf.UserIdFollower.Equals(user.UserId) &&
                cf.QaAIdFollow == QaAId);
            if(checkFollow != null)
            {
                var UN = await _context.UserNotificationss.FirstOrDefaultAsync(un => un.QaAFollowersId == checkFollow.QaAFollowersId);
                _context.UserNotificationss.Remove(UN);
                await _context.SaveChangesAsync();

                _context.QaAFollowerss.Remove(checkFollow);
                await _context.SaveChangesAsync();
                return new APIResponse
                {
                    Success = true,
                    Message = "Unfollow successfully.",
                };
            }

            var newFollower = new QaAFollowers
            {
                UserIdFollower = user.UserId,
                QaAIdFollow = QaAId
            };
            await _context.AddAsync(newFollower);
            await _context.SaveChangesAsync();

            var follow = await _context.QaAFollowerss
                .Include(u => u.QuestionAndAnswerNavigation)
                .FirstOrDefaultAsync(f => f.QaAFollowersId == newFollower.QaAFollowersId);
            var newUN = new UserNotificationModelCreate
            {
                UserNotificationsContent = $"{user.FirstName+ " " + user.LastName} đã theo dõi bình luận của bạn",
                UserIdNotifications = follow.QuestionAndAnswerNavigation.UserIdComment,
                QaAFollowersId = newFollower.QaAFollowersId
            };

            await _interfaceUNRepository.CreateUN(newUN);
                
            return new APIResponse
            {
                Success = true,
                Message = "Follow successfully.",
            };
        }

        #endregion
        //PUT
        #region
        public async Task<APIResponse> QaALike(int QaAId)
        {
            var QaA = await _context.QuestionAndAnswers.FindAsync(QaAId);
            QaA.countLike += 1;
            await _context.SaveChangesAsync();
            return new APIResponse { Success = true, Message = $"QaA Like Ok {QaA.QuestionAndAnswerId}" };
        }      
        #endregion
    }
}
