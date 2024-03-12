using AutoMapper;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.LessonResources;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LearningManagementSystem.Repository
{
    public class LessonResourcesRepository : InterfaceLessonResourcesRepository
    {
        private readonly LearningManagementSystemContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public LessonResourcesRepository(LearningManagementSystemContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._mapper = mapper;
            this._httpContextAccessor = httpContextAccessor;
        }
        //private

        //GET
        #region
        public async Task<APIResponse> GetObjectByLessonId(string lessonId)
        {
            var obj = await _context.LessonResourcess
                .Include(lar => lar.LecturesAndResourcesNavigation)
                .Include(l => l.LessonNavigation)
                    .ThenInclude(l => l.SubjectTopicNavigation)
                .Include(c => c.ClassNavigation)
                .Where(lr => lr.LessonId == int.Parse(lessonId))
                .Where(lr => lr.LecturesAndResourcesNavigation.Approve == true)
                .Where(lr => lr.LecturesAndResourcesNavigation.TypeOfDocument.Equals("Bài giảng"))
                .ToListAsync();
            return new APIResponse
            {
                Success = true,
                Message = "Had found",
                Data = _mapper.Map<List<LessonResourcesView>>(obj)
            };
        }
        public async Task<APIResponse> GetObjectByLessonIdAndClassId(string lessonId, string classId)
        {
            var obj = await _context.LessonResourcess
                .Include(lar => lar.LecturesAndResourcesNavigation)
                .Include(l => l.LessonNavigation)
                    .ThenInclude(l => l.SubjectTopicNavigation)
                .Include(c => c.ClassNavigation)
                .Where(lr => lr.ClassId.Equals(Guid.Parse(classId)))
                .Where(lr => lr.LessonId == int.Parse(lessonId))
                .Where(lr => lr.LecturesAndResourcesNavigation.Approve == true)
                .Where(lr => lr.LecturesAndResourcesNavigation.TypeOfDocument.Equals("Bài giảng"))
                .ToListAsync();
            return new APIResponse
            {
                Success = true,
                Message = "Had found",
                Data = _mapper.Map<List<LessonResourcesView>>(obj)
            };
        }
        public async Task<APIResponse> GetListLecturesAndResourcesIdBySubjectTopicIdAndClassId(int subjectTopicId, string classId)
        {
            var user = await _context.Users.Include(role => role.UserRole)
                           .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
            //từ TopicId lấy ra SubjectId
            var subjectTopic = await _context.SubjectTopics
                .Include(s => s.SubjectIdNavigation)
                .FirstOrDefaultAsync(st => st.Id == subjectTopicId);


            if (subjectTopic != null && subjectTopic.SubjectIdNavigation != null)
            {
                var subjectId = subjectTopic.SubjectIdNavigation?.SubjectId;

                var userInClass = await _context.UserClassSubjects
                .FirstOrDefaultAsync(u => u.UserId.Equals(user.UserId)
                && u.ClassId.Equals(Guid.Parse(classId))
                && u.SubjectId.Equals(subjectId));
                if (userInClass == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = $"No users were found who studied {subjectId} this subject in class {classId}"
                    };
                }
                var lessonResources = await _context.LessonResourcess
                        .Include(lr => lr.LessonNavigation)
                            .ThenInclude(l => l.SubjectTopicNavigation)
                        .Where(lr => lr.ClassId == Guid.Parse(classId) &&
                                     lr.LessonId == lr.LessonNavigation.LessonId &&
                                        lr.LessonNavigation.SubjectTopicNavigation.Id == subjectTopicId)
                        .ToListAsync();
                List<int> listFileId = new List<int>();
                foreach (var obj in lessonResources)
                {
                    listFileId.Add((int)obj.LecturesAndResourcesId);
                }
                return new APIResponse
                {
                    Success = true,
                    Message = "List fileId",
                    Data = new FileIdAndTopicTitle
                    {
                        TopicTitle = subjectTopic.SubjectTopicTitle,
                        FileId = listFileId
                    }
                };
            }
            else
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "subjectTopic or its navigation property is null"
                };
            }
        }

        #endregion
        //POST
        #region
        public async Task<APIResponse> CreateNewLecturesAndAddFileToLecture(LessonLectureModelCreate model)
        {
            try
            {
                var lecture = await _context.LecturesAndResourcesL.FirstOrDefaultAsync(
                    l => l.Id == model.LecturesAndResourcesId
                    && l.TypeOfDocument.Equals("Bài giảng")
                );
                if (lecture == null)
                {
                    return new APIResponse
                    {
                        Success = true,
                        Message = "Lecture file not found."
                    };
                }
                var nonExistingClassIds = model.ClassId.Where(classId =>
                    !_context.Classs.Any(c => c.ClassId == classId)).ToList();

                if (nonExistingClassIds.Any())
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = $"The following ClassId does not exist in the database: {string.Join(", ", nonExistingClassIds)}"
                    };
                }
                var newLesson = _mapper.Map<Lesson>(model);
                await _context.AddAsync(newLesson);
                await _context.SaveChangesAsync();
                // Liên kết bài học với các lớp học đã chọn
                if (model.ClassId != null && model.ClassId.Any())
                {
                    foreach (var classId in model.ClassId)
                    {
                        var lessonClass = new LessonResources
                        {
                            LessonId = newLesson.LessonId,
                            ClassId = classId,
                            LecturesAndResourcesId = model.LecturesAndResourcesId
                        };
                        await _context.AddAsync(lessonClass);
                    }
                    await _context.SaveChangesAsync();
                }
                return new APIResponse
                {
                    Success = true,
                    Message = "CreateNewLecturesAndAddFileToLecture success."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Error {ex.Message}",
                };
            }
        }

        public async Task<APIResponse> AddResourcesToLecture(LessonResourcesModelCreate model)
        {
            try
            {
                var nonExistingClassIds = model.ClassId.Where(classId =>
                    !_context.Classs.Any(c => c.ClassId == classId)).ToList();

                if (nonExistingClassIds.Any())
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = $"The following ClassId does not exist in the database: {string.Join(", ", nonExistingClassIds)}"
                    };
                }
                var nonExistingLecturesAndResourcesIds = model.LecturesAndResourcesId.Where(lrId =>
                    !_context.LecturesAndResourcesL.Any(lr => lr.Id == lrId && lr.TypeOfDocument.Equals("Tài nguyên"))).ToList();
                if (nonExistingLecturesAndResourcesIds.Any())
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = $"The following LecturesAndResourcesId does not exist in the database: {string.Join(", ", nonExistingLecturesAndResourcesIds)}"
                    };
                }
                var lesson = await _context.Lessons.FirstOrDefaultAsync(l => l.LessonId == model.LessonId);
                if (lesson == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "LessonId not found."
                    };
                }
                var newLessonResources = new List<LessonResources>();
                foreach (var lrId in model.LecturesAndResourcesId)
                {
                    foreach (var classId in model.ClassId)
                    {
                        var lessonResource = new LessonResources
                        {
                            LessonId = model.LessonId,
                            LecturesAndResourcesId = lrId,
                            ClassId = classId
                        };

                        newLessonResources.Add(lessonResource);
                    }
                }
                // Thêm danh sách bài giảng mới vào CSDL
                await _context.LessonResourcess.AddRangeAsync(newLessonResources);
                await _context.SaveChangesAsync();
                return new APIResponse
                {
                    Success = true,
                    Message = "AddResourcesToLecture success."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Error {ex.Message}",
                };
            }
        }

        public async Task<APIResponse> AddLectureAndFileToClasses(LessonResourcesModelCreate model)
        {
            try
            {
                var nonExistingClassIds = model.ClassId.Where(classId =>
                    !_context.Classs.Any(c => c.ClassId == classId)).ToList();

                if (nonExistingClassIds.Any())
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = $"The following ClassId does not exist in the database: {string.Join(", ", nonExistingClassIds)}"
                    };
                }
                var nonExistingLecturesAndResourcesIds = model.LecturesAndResourcesId.Where(lrId =>
                    !_context.LecturesAndResourcesL.Any(lr => lr.Id == lrId && lr.TypeOfDocument.Equals("Bài giảng"))).ToList();
                if (nonExistingLecturesAndResourcesIds.Any())
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = $"The following LecturesAndResourcesId does not exist in the database: {string.Join(", ", nonExistingLecturesAndResourcesIds)}"
                    };
                }
                var lesson = await _context.Lessons.FirstOrDefaultAsync(l => l.LessonId == model.LessonId);
                if (lesson == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "LessonId not found."
                    };
                }
                var newLessonResources = new List<LessonResources>();
                foreach (var lrId in model.LecturesAndResourcesId)
                {
                    foreach (var classId in model.ClassId)
                    {
                        var lessonResource = new LessonResources
                        {
                            LessonId = model.LessonId,
                            LecturesAndResourcesId = lrId,
                            ClassId = classId
                        };

                        newLessonResources.Add(lessonResource);
                    }
                }
                // Thêm danh sách bài giảng mới vào CSDL
                await _context.LessonResourcess.AddRangeAsync(newLessonResources);
                await _context.SaveChangesAsync();
                return new APIResponse
                {
                    Success = true,
                    Message = "AddLectureAndFileToClasses success."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Error {ex.Message}",
                };
            }
        }
        #endregion
        //PUT
        #region
        #endregion
        //DELETE
        #region
        #endregion
    }
}
