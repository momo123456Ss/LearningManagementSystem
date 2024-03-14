using AutoMapper;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.LessonModel;
using LearningManagementSystem.Models.SubjectTopicModel;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Repository
{
    public class SubjectTopicRepository : InterfaceSubjectTopicRepository
    {
        private readonly LearningManagementSystemContext _context;
        private readonly IMapper _mapper;
        public SubjectTopicRepository(LearningManagementSystemContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }
        //GET đoạn nào?
        #region
        public async Task<APIResponse> GetById(string subjectTopicId, string classId)
        {
            #region
            //var lessons =  _context.Lessons
            //        .Include(lr => lr.LessonResourcess)
            //        .Where(lesson => lesson.SubjectTopicId == int.Parse(subjectTopicId))
            //        .AsQueryable();

            //var lessonResource = await _context.LessonResourcess
            //    .Include(lar => lar.LecturesAndResourcesNavigation)
            //    .Where(lr => lr.ClassId.ToString().ToLower().Equals(classId.ToLower()) &&
            //    lr.LecturesAndResourcesNavigation.Approve == true)
            //    .ToListAsync();
            //lessons = lessons.Where(l => l.LessonResourcess.Any(lr => lessonResource.Contains(lr)));
            #endregion
            #region
            var lessons = (from l in _context.Lessons
                           join lr in _context.LessonResourcess on l.LessonId equals lr.LessonId
                           join st in _context.SubjectTopics on l.SubjectTopicId equals st.Id
                           join lar in _context.LecturesAndResourcesL on lr.LecturesAndResourcesId equals lar.Id
                           where st.Id == int.Parse(subjectTopicId) &&
                                 lr.ClassId == Guid.Parse(classId) &&
                                 lar.Approve == true
                           select new Lesson
                           {
                               LessonId = l.LessonId,
                               LessonTitle = l.LessonTitle,
                               LessonResourcess = l.LessonResourcess,
                               //ClassId = lr.ClassId,
                               //LecturesAndResourcesId = lar.Id,
                               //FileName = lar.FileName,
                               //FileUrl = lar.FileUrl
                           });
            #endregion
            var result = await lessons.Distinct().ToListAsync();
            return new APIResponse
            {
                Success = true,
                Message = "Had found.",
                Data = result /*_mapper.Map<List<LessonModelView2>>(result)*/
            };
        }
        #endregion
        //POST
        #region
        public async Task<APIResponse> CreateNew(SubjectTopicModelCreate model)
        {
            try
            {
                var subject = await _context.Subjects.FindAsync(model.SubjectId);

                if (subject == null)
                {
                    return new APIResponse { Success = false, Message = "Subject not found" };
                }
                var newSubjectTopic = _mapper.Map<SubjectTopic>(model);
                await _context.AddAsync(newSubjectTopic);
                await _context.SaveChangesAsync();
                return new APIResponse
                {
                    Success = true,
                    Message = "Created successfully.",
                    Data = _mapper.Map<SubjectTopicModelView>(newSubjectTopic)
                };
            }
            catch (Exception ex)
            {
                return new APIResponse { Success = false, Message = $"Error CreateNew SubjectTopic: {ex.Message}" };
            }
        }
        #endregion
        //PUT
        #region
        public async Task<APIResponse> UpdateById(string id, SubjectTopicModelUpdate model)
        {
            try
            {
                var subjectTopic = await _context.SubjectTopics.FindAsync(int.Parse(id));

                if (subjectTopic == null)
                {
                    return new APIResponse { Success = false, Message = "Subject not found" };
                }
                // Cập nhật từng trường một từ model vào userRole
                #region
                foreach (var property in typeof(SubjectTopicModelUpdate).GetProperties())
                {
                    var modelValue = property.GetValue(model);
                    if (modelValue != null)
                    {
                        var subjectTopicProperty = typeof(SubjectTopic).GetProperty(property.Name);
                        if (subjectTopicProperty != null)
                        {
                            subjectTopicProperty.SetValue(subjectTopic, modelValue);
                        }
                    }
                }
                #endregion
                await _context.SaveChangesAsync();

                return new APIResponse { Success = true, Message = "SubjectTopic updated successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse { Success = false, Message = $"Error updating SubjectTopic: {ex.Message}" };
            }
        }
        #endregion
        //DELETE
        #region
        public Task<APIResponse> DeleteById(string id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
