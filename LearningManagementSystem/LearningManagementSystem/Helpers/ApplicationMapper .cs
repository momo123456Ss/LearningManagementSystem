using AutoMapper;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.UserModel;
using LearningManagementSystem.Models.UserRoleModels;

namespace LearningManagementSystem.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() {
            CreateMap<UserRole, UserRoleModel>().ReverseMap();
            CreateMap<UserRole, UserRoleViewModel>().ReverseMap();
            CreateMap<User, UserModelAllType>().ReverseMap();
            CreateMap<User, UserModelUpdateAllType>().ReverseMap();

        }
    }
}
