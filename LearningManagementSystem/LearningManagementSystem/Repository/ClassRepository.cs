using AutoMapper;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.ClassModel;
using LearningManagementSystem.Models.UserRoleModels;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;

namespace LearningManagementSystem.Repository
{
    public class ClassRepository : InterfaceClassRepository
    {
        private readonly LearningManagementSystemContext _context;
        private readonly IMapper _mapper;

        public ClassRepository(LearningManagementSystemContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }
        //GET
        #region
        public async Task<List<ClassModelView>> GetAllClassOpenDateStartBetween(string startString, string endString)
        {
            if (!DateTime.TryParseExact(startString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime start))
            {
                throw new ArgumentException("Invalid start date format. Please use yyyy-MM-dd format.");
            }

            if (!DateTime.TryParseExact(endString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime end))
            {
                throw new ArgumentException("Invalid end date format. Please use yyyy-MM-dd format.");
            }

            var classesStartBetween = await _context.Classs
                .Where(c => c.ClassOpeningDay >= start && c.ClassOpeningDay <= end).ToListAsync();
            return _mapper.Map<List<ClassModelView>>(classesStartBetween);
        }
        public async Task<List<ClassModelView>> GetAllClassHasNotYetEnded()
        {
            var classesHasEnded = await _context.Classs
                .Where(c => c.ClassClosingDay > DateTime.Now && c.ClassClosingDay != DateTime.MinValue).ToListAsync();
            return _mapper.Map<List<ClassModelView>>(classesHasEnded);
        }
        public async Task<List<ClassModelView>> GetAllClassHasEnded()
        {
            var classesHasEnded = await _context.Classs
                .Where(c => c.ClassClosingDay < DateTime.Now && c.ClassClosingDay != DateTime.MinValue).ToListAsync();
            return _mapper.Map<List<ClassModelView>>(classesHasEnded);
        }
        public async Task<List<ClassModelView>> GetAllClassDoesNotHaveAnEndDateYet()
        {
            var classesWithoutEndDate = await _context.Classs
                .Where(c => c.ClassClosingDay == DateTime.MinValue).ToListAsync();
            return _mapper.Map<List<ClassModelView>>(classesWithoutEndDate);
        }

        public async Task<List<ClassModelView>> GetAll()
        {
            return _mapper.Map<List<ClassModelView>>(await _context.Classs.ToListAsync());
        }

        public async Task<APIResponse> GetById(string id)
        {

            try
            {
                return new APIResponse
                {
                    Success = true,
                    Message = "Had found.",
                    Data = _mapper.Map<ClassModelView>(await _context.Classs.FindAsync(Guid.Parse(id)))
                };
            }
            catch
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Not found.",
                };
            }
        }
        #endregion
        //POST
        #region
        public async Task<APIResponse> CreateNewClass(ClassModelCreate model)
        {
            var newClass = _mapper.Map<Class>(model);
            newClass.ClassId = Guid.NewGuid();
            await _context.AddAsync(newClass);
            await _context.SaveChangesAsync();
            return new APIResponse
            {
                Success = true,
                Message = "Created successfully.",
                Data = newClass
            };
        }
        #endregion
        //PUT
        #region
        public async Task<APIResponse> UpdateById(string id, ClassModelUpdate model)
        {
            try
            {
                var classroom = await _context.Classs.FindAsync(Guid.Parse(id));

                if (classroom == null)
                {
                    return new APIResponse { Success = false, Message = "Class not found" };
                }

                // Cập nhật từng trường một từ model vào userRole
                #region
                foreach (var property in typeof(ClassModelUpdate).GetProperties())
                {
                    var modelValue = property.GetValue(model);
                    if (modelValue != null)
                    {
                        var userRoleProperty = typeof(Class).GetProperty(property.Name);
                        if (userRoleProperty != null)
                        {
                            userRoleProperty.SetValue(classroom, modelValue);
                        }
                    }
                }
                #endregion
                await _context.SaveChangesAsync();

                return new APIResponse { Success = true, Message = "Class updated successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse { Success = false, Message = $"Error updating Class: {ex.Message}" };
            }
        }
        #endregion
    }
}
