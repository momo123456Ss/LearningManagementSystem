using AutoMapper;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.ClassModel;
using LearningManagementSystem.Models.FacultyModel;
using LearningManagementSystem.Models.UserBelongToFacultyModel;
using LearningManagementSystem.Models.UserModel;
using LearningManagementSystem.Models.UserRoleModels;

namespace LearningManagementSystem.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            //UserRole
            #region
            CreateMap<UserRole, UserRoleModel>().ReverseMap();
            CreateMap<UserRole, UserRoleViewModel>().ReverseMap();
            #endregion
            //User
            #region
            CreateMap<User, UserModelAllType>().ReverseMap();
            CreateMap<User, UserModelUpdateAllType>().ReverseMap();
            CreateMap<User, UserModelUpdateRole>().ReverseMap();
            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.UserRoleViewModel, opt => opt.MapFrom(src => src.UserRole))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ReverseMap();
            CreateMap<User, LeadershipModelUpdateNotificationSettings>().ReverseMap();
            CreateMap<User, TeacherModelUpdateNotificationSettings>().ReverseMap();
            CreateMap<User, StudentModelUpdateNotificationSettings>().ReverseMap();
            #endregion
            //Class
            #region
            CreateMap<Class, ClassModelView>().ReverseMap();
            CreateMap<Class, ClassModelCreate>().ReverseMap();
            CreateMap<Class, ClassModelUpdate>().ReverseMap();
            #endregion
            //Faculty
            #region
            CreateMap<Faculty, FacultyModelView>().ReverseMap();
            CreateMap<Faculty, FacultyModelCreate>().ReverseMap();
            CreateMap<Faculty, FacultyModelUpdate>().ReverseMap();
            #endregion
            //UserBelongToFaculty
            #region
            CreateMap<UserBelongToFaculty, UserBelongToFacultyModelCreate>().ReverseMap();
            #endregion

        }
    }
}
