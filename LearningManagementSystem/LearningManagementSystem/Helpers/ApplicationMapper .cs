using AutoMapper;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.ClassModel;
using LearningManagementSystem.Models.FacultyModel;
using LearningManagementSystem.Models.LecturesAndResourcesModel;
using LearningManagementSystem.Models.LessonModel;
using LearningManagementSystem.Models.LessonResources;
using LearningManagementSystem.Models.OtherSubjectInformationModel;
using LearningManagementSystem.Models.QaAModel;
using LearningManagementSystem.Models.SubjectModel;
using LearningManagementSystem.Models.SubjectTopicModel;
using LearningManagementSystem.Models.UserBelongToFacultyModel;
using LearningManagementSystem.Models.UserClassSubjectModel;
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
            CreateMap<Subject, Subject_LecturesAndResourcesModelView>()
                .ForMember(dest => dest.Lecturer, opt => opt.MapFrom(src => src.UserNavigation))
                .ReverseMap();
            CreateMap<Subject, SubjectModelView>()
                .ForMember(dest => dest.Lecturer, opt => opt.MapFrom(src => src.UserNavigation))
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
            CreateMap<SubjectTopic, SubjectTopicModelView>()
                .ForMember(dest => dest.LessonNavigation, opt => opt.MapFrom(src => src.Lessons))
                .ReverseMap();
            CreateMap<SubjectTopic, SubjectTopicModelCreate>().ReverseMap();
            CreateMap<SubjectTopic, SubjectTopicModelUpdate>().ReverseMap();
            #endregion
            //UserClassSubject
            #region
            CreateMap<UserClassSubject, UserClassSubjectModelView>()
                .ForMember(dest => dest.ClassNavigation, opt => opt.MapFrom(src => src.ClassNavigation))
                .ForMember(dest => dest.SubjectNavigation, opt => opt.MapFrom(src => src.SubjectNavigation))
                .ReverseMap();
            CreateMap<UserClassSubject, UserClassSubjectModelCreate>().ReverseMap();
            #endregion
            //LecturesAndResources
            #region
            CreateMap<LecturesAndResources, LecturesAndResourcesModelCreate>().ReverseMap();
            CreateMap<LecturesAndResources, LecturesAndResourcesModelDowload>().ReverseMap();
            CreateMap<LecturesAndResources, LecturesAndResourcesModelView>()
                .ForMember(dest => dest.SubjectNavigation, opt => opt.MapFrom(src => src.SubjectNavigation))
                .ReverseMap();
            CreateMap<LecturesAndResources, LecturesAndResourcesModelLessonView>().ReverseMap();
            #endregion
            //Lesson
            #region
            CreateMap<Lesson, LessonLectureModelCreate>().ReverseMap();
            CreateMap<Lesson, LessonModelView>().ReverseMap();
            CreateMap<Lesson, LessonModelCreate>().ReverseMap();
            #endregion
            //LessonResources
            #region
            CreateMap<LessonResources, LessonResourcesView>()
                .ForMember(dest => dest.LecturesAndResourcesNavigation, opt => opt.MapFrom(src => src.LecturesAndResourcesNavigation))
                .ReverseMap();
            #endregion
            //QaA
            #region
            CreateMap<QuestionAndAnswer, QaAModelCreate>()
                .ReverseMap();
            CreateMap<QuestionAndAnswer, QaAModelView>()
               .ReverseMap();
            CreateMap<QuestionAndAnswer, QaAModelUserView>()
               .ForMember(dest => dest.UserViewModelNavigation, opt => opt.MapFrom(src => src.UserNavigation))
               .ForMember(dest => dest.ClassModelViewNavigation, opt => opt.MapFrom(src => src.ClassNavigation))
               .ForMember(dest => dest.LessonModelViewNavigation, opt => opt.MapFrom(src => src.LessonNavigation))
               .ReverseMap();
            #endregion
        }
    }
}
