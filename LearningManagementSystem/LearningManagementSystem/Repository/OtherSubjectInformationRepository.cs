using AutoMapper;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.OtherSubjectInformationModel;
using LearningManagementSystem.Models.SubjectModel;
using LearningManagementSystem.Repository.InterfaceRepository;

namespace LearningManagementSystem.Repository
{
    public class OtherSubjectInformationRepository: InterfaceOtherSubjectInformationRepository
    {
        private readonly LearningManagementSystemContext _context;
        private readonly IMapper _mapper;
        public OtherSubjectInformationRepository(LearningManagementSystemContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }
        //GET
        #region
        #endregion
        //POST
        #region
        public async Task<APIResponse> CreateNew(OtherSubjectInformationModelCreate model)
        {
            var subject = await _context.Subjects.FindAsync(model.SubjectId);
            if (subject == null)
            {
                return new APIResponse { Success = false, Message = "Subject not found" };
            }
            var newOtherSubjectInformation = _mapper.Map<OtherSubjectInformation>(model);
            await _context.AddAsync(newOtherSubjectInformation);
            await _context.SaveChangesAsync();
            return new APIResponse
            {
                Success = true,
                Message = "Created successfully.",
                Data = _mapper.Map<OtherSubjectInformationModelView>(newOtherSubjectInformation)
            };
        }
        #endregion
        //PUT
        #region
        public async Task<APIResponse> UpdateById(string id, OtherSubjectInformationModelUpdate model)
        {
            try
            {
                var otherSubjectInformations = await _context.OtherSubjectInformations.FindAsync(int.Parse(id));

                if (otherSubjectInformations == null)
                {
                    return new APIResponse { Success = false, Message = "OtherSubjectInformations not found" };
                }
                // Kiểm tra nếu cả hai trường OtherSubjectTitle và OtherSubjectDescription là rỗng thì xóa bản ghi
                if ((model.OtherSubjectTitle != null && model.OtherSubjectTitle.Trim() == "") && (model.OtherSubjectDescription != null && model.OtherSubjectDescription.Trim() == ""))
                {
                    _context.OtherSubjectInformations.Remove(otherSubjectInformations);
                    await _context.SaveChangesAsync();
                    return new APIResponse { Success = true, Message = "OtherSubjectInformation deleted successfully" };
                }
                // Cập nhật từng trường một từ model vào userRole
                #region
                foreach (var property in typeof(OtherSubjectInformationModelUpdate).GetProperties())
                {
                    var modelValue = property.GetValue(model);
                    if (modelValue != null)
                    {
                        var otherSubjectInformationModelUpdateProperty = typeof(OtherSubjectInformation).GetProperty(property.Name);
                        if (otherSubjectInformationModelUpdateProperty != null)
                        {
                            otherSubjectInformationModelUpdateProperty.SetValue(otherSubjectInformations, modelValue);
                        }
                    }
                }
                #endregion
                await _context.SaveChangesAsync();

                return new APIResponse { Success = true, Message = "OtherSubjectInformation updated successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse { Success = false, Message = $"Error updating OtherSubjectInformation: {ex.Message}" };
            }
        }
        #endregion
    }
}
