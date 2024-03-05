using AutoMapper;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.FacultyModel;
using LearningManagementSystem.Models.PaginatedList;
using LearningManagementSystem.Models.UserModel;
using LearningManagementSystem.Models.UserRoleModels;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace LearningManagementSystem.Repository
{
    public class FacultyRepository : InterfaceFacultyRepository
    {
        private readonly LearningManagementSystemContext _context;
        private readonly IMapper _mapper;
        private static int PAGE_SIZE { get; set; } = 5;

        public FacultyRepository(LearningManagementSystemContext context, IMapper mapper) {
            this._mapper = mapper;
            this._context = context;
        }
        //GET
        #region
        public async Task<List<FacultyModelView>> GetAll(string searchString, string sortBy, int page = 1)
        {
            var allFaculty = _context.Facultys.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                allFaculty = allFaculty.Where(f =>
                    f.FacultyCode.ToLower().Contains(searchString) ||
                    f.FacultyName.ToLower().Contains(searchString)
                );
            }
            allFaculty = allFaculty.OrderBy(hh => hh.FacultyName);
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "fname_asc":
                        allFaculty = allFaculty.OrderBy(f => f.FacultyName);
                        break;
                    case "fname_desc":
                        allFaculty = allFaculty.OrderByDescending(f => f.FacultyName);
                        break;
                    case "fcode_asc":
                        allFaculty = allFaculty.OrderBy(f => f.FacultyCode);
                        break;
                    case "fcode_desc":
                        allFaculty = allFaculty.OrderByDescending(f => f.FacultyCode);
                        break;
                    case "festablishmentdate_asc":
                        allFaculty = allFaculty.OrderBy(f => f.EstablishmentDate);
                        break;
                    case "festablishmentdate_desc":
                        allFaculty = allFaculty.OrderByDescending(f => f.EstablishmentDate);
                        break;
                }
            }

            if (page < 1)
            {
                return _mapper.Map<List<FacultyModelView>>(allFaculty);
            }
            var result = PaginatedList<Faculty>.Create(allFaculty, page, PAGE_SIZE);
            return _mapper.Map<List<FacultyModelView>>(result);
        }
        public async Task<APIResponse> GetById(string id)
        {
            try
            {
                return new APIResponse
                {
                    Success = true,
                    Message = "Had found.",
                    Data = _mapper.Map<FacultyModelView>(await _context.Facultys.FindAsync(Guid.Parse(id)))
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
        public async Task<APIResponse> CreateNew(FacultyModelCreate model)
        {
            // Kiểm tra xem tên vai trò đã tồn tại hay chưa
            if (await _context.Facultys.AnyAsync(f => f.FacultyName == model.FacultyName 
            || f.FacultyCode == model.FacultyCode))
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "FacultyName or FacultyCode exists"
                };
            }
            var newFaculty = _mapper.Map<Faculty>(model);
            newFaculty.FacultyId = Guid.NewGuid();
            newFaculty.EstablishmentDate = DateTime.Now;
            await _context.AddAsync(newFaculty);
            await _context.SaveChangesAsync();
            return new APIResponse
            {
                Success = true,
                Message = "Created successfully.",
                Data = _mapper.Map<FacultyModelView>(newFaculty)
        };
        }
        #endregion
        //PUT
        #region
        public async Task<APIResponse> UpdateById(string id, FacultyModelUpdate model)
        {
            try
            {
                var faculty = await _context.Facultys.FindAsync(Guid.Parse(id));

                if (faculty == null)
                {
                    return new APIResponse { Success = false, Message = "Faculty not found" };
                }
                // Cập nhật từng trường một từ model vào userRole
                #region
                foreach (var property in typeof(FacultyModelUpdate).GetProperties())
                {
                    var modelValue = property.GetValue(model);
                    if (modelValue != null)
                    {
                        var facultyProperty = typeof(Faculty).GetProperty(property.Name);
                        if (facultyProperty != null)
                        {
                            facultyProperty.SetValue(faculty, modelValue);
                        }
                    }
                }
                #endregion
                await _context.SaveChangesAsync();

                return new APIResponse { Success = true, Message = "Faculty updated successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse { Success = false, Message = $"Error updating Faculty: {ex.Message}" };
            }
        }
        #endregion
    }
}
