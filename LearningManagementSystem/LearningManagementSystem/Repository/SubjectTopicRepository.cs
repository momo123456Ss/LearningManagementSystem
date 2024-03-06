using AutoMapper;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.SubjectModel;
using LearningManagementSystem.Models.SubjectTopicModel;
using LearningManagementSystem.Repository.InterfaceRepository;

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
        //GET
        #region
        public Task<APIResponse> GetById(string id)
        {
            throw new NotImplementedException();
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
