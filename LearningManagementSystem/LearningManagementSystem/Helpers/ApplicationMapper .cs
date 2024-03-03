using AutoMapper;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.UserModel;
using LearningManagementSystem.Models.UserRoleModels;

namespace LearningManagementSystem.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            //UserRole
            CreateMap<UserRole, UserRoleModel>().ReverseMap();
            CreateMap<UserRole, UserRoleViewModel>().ReverseMap();
            //User
            CreateMap<User, UserModelAllType>().ReverseMap();
            CreateMap<User, UserModelUpdateAllType>().ReverseMap();
            CreateMap<User, UserModelUpdateRole>().ReverseMap();
            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.UserRoleViewModel, opt => opt.MapFrom(src => src.UserRole))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ReverseMap();


        }
    }
}
