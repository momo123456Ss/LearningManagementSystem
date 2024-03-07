using AutoMapper;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.ClassModel;
using LearningManagementSystem.Models.FacultyModel;
using LearningManagementSystem.Models.OtherSubjectInformationModel;
using LearningManagementSystem.Models.SubjectModel;
using LearningManagementSystem.Models.SubjectTopicModel;
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
            CreateMap<Class, ClassModelView>()
                .ForMember(dest => dest.FacultyModelViewNavigation, opt => opt.MapFrom(src => src.FacultyNavigation))
                .ReverseMap();
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
            //Subject
            #region
            CreateMap<Subject, SubjectModelView>()
                .ForMember(dest => dest.UserViewModel, opt => opt.MapFrom(src => src.UserNavigation))
                .ForMember(dest => dest.OtherSubjectInformationModelViews, opt => opt.MapFrom(src => src.OtherSubjectInformations))
                .ForMember(dest => dest.SubjectTopics, opt => opt.MapFrom(src => src.SubjectTopics))
                .ReverseMap();
            CreateMap<Subject, SubjectModelCreate>().ReverseMap();
            CreateMap<Subject, SubjectModelUpdate>().ReverseMap();
            #endregion

            //OtherSubjectInformation
            #region
            CreateMap<OtherSubjectInformation, OtherSubjectInformationModelView>().ReverseMap();
            CreateMap<OtherSubjectInformation, OtherSubjectInformationModelCreate>().ReverseMap();
            CreateMap<OtherSubjectInformation, OtherSubjectInformationModelUpdate>().ReverseMap();
            #endregion
            //SubjectTopic
            #region
            CreateMap<SubjectTopic, SubjectTopicModelView>().ReverseMap();
            CreateMap<SubjectTopic, SubjectTopicModelCreate>().ReverseMap();
            CreateMap<SubjectTopic, SubjectTopicModelUpdate>().ReverseMap();
            #endregion
        }
    }
}
