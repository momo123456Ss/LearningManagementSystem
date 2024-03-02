using AutoMapper;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.UserRoleModels;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Repository
{
    public class UserRoleRepository : InterfaceUserRoleRepository
    {
        private readonly LearningManagementSystemContext _context;
        private readonly IMapper _mapper;

        public UserRoleRepository(LearningManagementSystemContext context, IMapper mapper) { 
            this._context = context;
            this._mapper = mapper;
        }
        public async Task<APIResponse> CreateNew(UserRoleModel model)
        {
            // Kiểm tra xem tên vai trò đã tồn tại hay chưa
            if (await _context.UserRoles.AnyAsync(u => u.RoleName == model.RoleName))
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Role name exists"
                };
            }
            var newRole = _mapper.Map<UserRole>(model);
            newRole.RoleId = Guid.NewGuid();
            await _context.AddAsync(newRole);
            await _context.SaveChangesAsync();
            return new APIResponse
            {
                Success = true,
                Message = "Created successfully.",
                Data = newRole
            };
        }

        public async Task<APIResponse> DeleteById(string id)
        {
            try
            {
                var userRole = await _context.UserRoles.FindAsync(Guid.Parse(id));

                if (userRole == null)
                {
                    return new APIResponse { Success = false, Message = "UserRole not found" };
                }

                _context.UserRoles.Remove(userRole);
                await _context.SaveChangesAsync();

                return new APIResponse { Success = true, Message = "UserRole deleted successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse { Success = false, Message = $"Error deleting UserRole: {ex.Message}" };
            }
        }

        public async Task<List<UserRoleViewModel>> GetAll()
        {
            return _mapper.Map<List<UserRoleViewModel>>(await _context.UserRoles.ToListAsync());
        }

        public async Task<APIResponse> GetById(string id)
        {
            try
            {
                return new APIResponse
                {
                    Success = true,
                    Message = "Had found.",
                    Data = _mapper.Map<UserRoleModel>(await _context.UserRoles.FindAsync(Guid.Parse(id)))
                };
            }catch
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Not found.",                  
                };
            }

        }

        public async Task<APIResponse> UpdateById(string id, UserRoleModel model)
        {
            try
            {
                var userRole = await _context.UserRoles.FindAsync(Guid.Parse(id));

                if (userRole == null)
                {
                    return new APIResponse { Success = false, Message = "UserRole not found" };
                }
                //_mapper.Map(model, userRole);

                // Cập nhật từng trường một từ model vào userRole
                #region
                foreach (var property in typeof(UserRoleModel).GetProperties())
                {
                    var modelValue = property.GetValue(model);
                    if (modelValue != null)
                    {
                        var userRoleProperty = typeof(UserRole).GetProperty(property.Name);
                        if (userRoleProperty != null)
                        {
                            userRoleProperty.SetValue(userRole, modelValue);
                        }
                    }
                }
                #endregion
                await _context.SaveChangesAsync();

                return new APIResponse { Success = true, Message = "UserRole updated successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse { Success = false, Message = $"Error updating UserRole: {ex.Message}" };
            }
        }
    }
}
