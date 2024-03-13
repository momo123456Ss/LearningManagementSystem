using AutoMapper;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.LessonModel;
using LearningManagementSystem.Repository.InterfaceRepository;

namespace LearningManagementSystem.Repository
{
    public class LessonRepository : InterfaceLessonRepository
    {
        private readonly LearningManagementSystemContext _context;
        private readonly IMapper _mapper;
        public LessonRepository(LearningManagementSystemContext context, IMapper mapper) { 
            _context = context;
            _mapper = mapper;
        }
        //GET
        #region
        #endregion
        //POST
        #region
        public async Task<APIResponse> CreateLesson(LessonModelCreate model)
        {
            var subjectTopic = await _context.SubjectTopics.FindAsync(model.SubjectTopicId);
            if(subjectTopic == null)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "subjectTopicId not found."
                };
            }
            //Kiểm tra List
            #region
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
            var nonExistingLecturesIds = model.LectureId.Where(lrId =>
                    !_context.LecturesAndResourcesL.Any(lr => lr.Id == lrId && lr.TypeOfDocument.Equals("Bài giảng"))).ToList();
            if (nonExistingLecturesIds.Any())
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"The following LectureId does not exist in the database: {string.Join(", ", nonExistingLecturesIds)}"
                };
            }
            var nonExistingResourcesIds = model.ResourceId.Where(lrId =>
                    !_context.LecturesAndResourcesL.Any(lr => lr.Id == lrId && lr.TypeOfDocument.Equals("Tài nguyên"))).ToList();
            if (nonExistingResourcesIds.Any())
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"The following nonExistingResourcesIds does not exist in the database: {string.Join(", ", nonExistingResourcesIds)}"
                };
            }
            #endregion
            //Tạo bài giảng
            var newLesson = _mapper.Map<Lesson>(model);
            await _context.AddAsync(newLesson);
            await _context.SaveChangesAsync();
            //Thêm tài liệu bài giảng tương ứng với các lớp
            var newLessonLectures = new List<LessonResources>();
            foreach (var lrId in model.LectureId)
            {
                foreach (var classId in model.ClassId)
                {
                    var lessonResource = new LessonResources
                    {
                        LessonId = newLesson.LessonId,
                        LecturesAndResourcesId = lrId,
                        ClassId = classId
                    };

                    newLessonLectures.Add(lessonResource);
                }
            }
            await _context.LessonResourcess.AddRangeAsync(newLessonLectures);
            await _context.SaveChangesAsync();
            //Thêm tài liệu tài nguyên tương ứng với các lớp
            var newLessonResources = new List<LessonResources>();
            foreach (var lrId in model.ResourceId)
            {
                foreach (var classId in model.ClassId)
                {
                    var lessonResource = new LessonResources
                    {
                        LessonId = newLesson.LessonId,
                        LecturesAndResourcesId = lrId,
                        ClassId = classId
                    };

                    newLessonResources.Add(lessonResource);
                }
            }
            await _context.LessonResourcess.AddRangeAsync(newLessonResources);
            await _context.SaveChangesAsync();
            return new APIResponse
            {
                Success = true,
                Message = "CreateLesson success."
            };
        }
        #endregion
        //PUT
        #region
        #endregion
    }
}
