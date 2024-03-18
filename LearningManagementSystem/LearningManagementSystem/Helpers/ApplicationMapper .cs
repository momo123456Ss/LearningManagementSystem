using AutoMapper;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.ClassModel;
using LearningManagementSystem.Models.ExemAndTest;
using LearningManagementSystem.Models.FacultyModel;
using LearningManagementSystem.Models.LecturesAndResourcesModel;
using LearningManagementSystem.Models.LessonModel;
using LearningManagementSystem.Models.LessonResources;
using LearningManagementSystem.Models.OtherSubjectInformationModel;
using LearningManagementSystem.Models.QaAFollwers;
using LearningManagementSystem.Models.QaAModel;
using LearningManagementSystem.Models.SAModel;
using LearningManagementSystem.Models.SubjectModel;
using LearningManagementSystem.Models.SubjectTopicModel;
using LearningManagementSystem.Models.UserBelongToFacultyModel;
using LearningManagementSystem.Models.UserClassSubjectModel;
using LearningManagementSystem.Models.UserModel;
using LearningManagementSystem.Models.UserNotificationsModel;
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
            CreateMap<User, UserLiteViewModel>()
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
            CreateMap<Faculty, FacultyLiteViewModel>().ReverseMap();
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
            CreateMap<Subject, SubjectLiteViewModel>().ReverseMap();
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
            CreateMap<SubjectTopic, SubjectTopicLiteViewModel>()
                .ForMember(dest => dest.SubjectIdNavigation, opt => opt.MapFrom(src => src.SubjectIdNavigation))
                .ReverseMap();
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
            CreateMap<Lesson, LessonModelView2>().ReverseMap();
            CreateMap<Lesson, LessonModelCreate>().ReverseMap();
            CreateMap<Lesson, LessonLiteViewModel>()
                .ForMember(dest => dest.SubjectTopicNavigation, opt => opt.MapFrom(src => src.SubjectTopicNavigation))
                .ReverseMap();
            #endregion
            //LessonResources
            #region
            CreateMap<LessonResources, LessonResourcesView>()
                .ForMember(dest => dest.LecturesAndResourcesNavigation, opt => opt.MapFrom(src => src.LecturesAndResourcesNavigation))
                .ReverseMap();
            #endregion
            //QaAFollow
            #region
            CreateMap<QaAFollowers, QaAFollowViewModel>()
                .ForMember(dest => dest.UserIdFollowerNavigation, opt => opt.MapFrom(src => src.UserIdFollowerNavigation))
                .ForMember(dest => dest.QuestionAndAnswerNavigation, opt => opt.MapFrom(src => src.QuestionAndAnswerNavigation))
                .ReverseMap();
            #endregion
            //QaA
            #region
            CreateMap<QuestionAndAnswer, QaALiteViewModel>()
                .ForMember(dest => dest.ClassModelViewNavigation, opt => opt.MapFrom(src => src.ClassNavigation))
                .ForMember(dest => dest.LessonModelViewNavigation, opt => opt.MapFrom(src => src.LessonNavigation))
                .ReverseMap();
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
            //SubjectAnnouncement
            #region
            CreateMap<SubjectAnnouncement, SubjectAnnouncementModelView>()
                .ForMember(dest => dest.UserModelViewNavigation, opt => opt.MapFrom(src => src.UserAnnouncementNavigation))
                .ForMember(dest => dest.ClassModelViewNavigation, opt => opt.MapFrom(src => src.ClassAnnouncementNavigation))
                .ReverseMap();
            CreateMap<SubjectAnnouncement, SubjectAnnouncementModelCreate>().ReverseMap();
            CreateMap<SubjectAnnouncement, SubjectAnnouncementModelCreateSingle>().ReverseMap();
            CreateMap<SubjectAnnouncement, SubjectAnnouncementLiteViewModel>()
                .ForMember(dest => dest.SubjectAnnouncementNavigation, opt => opt.MapFrom(src => src.SubjectAnnouncementNavigation))
                .ForMember(dest => dest.ClassModelViewNavigation, opt => opt.MapFrom(src => src.ClassAnnouncementNavigation))
                .ReverseMap();

            #endregion
            //UserNotificationModel
            #region
            CreateMap<UserNotifications, UserNotificationModelCreate>()
                .ReverseMap();
            CreateMap<UserNotifications, UserNotificationViewModel>()
                .ForMember(dest => dest.UserLiteViewModel, opt => opt.MapFrom(src => src.UserNotificationsNavigation))
                .ForMember(dest => dest.QaAFollowersNavigation, opt => opt.MapFrom(src => src.QaAFollowersNavigation))
                .ForMember(dest => dest.SubjectAnnouncementNavigation, opt => opt.MapFrom(src => src.SubjectAnnouncementNavigation))
                .ForMember(dest => dest.QuestionAndAnswerNavigation, opt => opt.MapFrom(src => src.QuestionAndAnswerNavigation))
                .ReverseMap();
            #endregion
            //ExamAndTest
            #region
            CreateMap<ExamAndTest, ExamAndTestModelUploadFile>().ReverseMap();
            CreateMap<ExamAndTest, ExamAndTestModelDowload>().ReverseMap();
            CreateMap<ExamAndTest, ExamAndTestViewModel>()
                .ForMember(dest => dest.SubjectNavigation, opt => opt.MapFrom(src => src.SubjectNavigation))
                .ForMember(dest => dest.FacultyNavigation, opt => opt.MapFrom(src => src.FacultyNavigation))
                .ReverseMap();
            #endregion
        }
    }
}
