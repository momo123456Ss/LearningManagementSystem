using AutoMapper;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.ExamAndTestQuestionModel;
using LearningManagementSystem.Models.UserModel;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LearningManagementSystem.Repository
{
    public class EaTQuestionRepository : InterfaceEaTQuestionRepository
    {
        private readonly LearningManagementSystemContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EaTQuestionRepository(LearningManagementSystemContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._mapper = mapper;
            this._httpContextAccessor = httpContextAccessor;
        }
        //Private      
        //GET
        #region
        #endregion
        //POST
        #region
        public async Task<APIResponse> CreateQuestions(List<ExamAndTestQuestionCreateModel> models)
        {
            var user = await _context.Users.Include(role => role.UserRole)
              .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
            if (!user.UserRole.ExamsAndTestsCreateNew)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "User dont have permission."
                };
            }
            foreach (var model in models)
            {
                // Check if SubjectId and FacultyId exist in the database
                var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.SubjectId.Equals(model.SubjectId));
                var faculty = await _context.Facultys.FirstOrDefaultAsync(f => f.FacultyId.Equals(model.FacultyId));

                if (subject == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = $"Invalid SubjectId for question: {model.ExamAndTestQuestionContent}"
                    };
                }

                if (faculty == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = $"Invalid FacultyId for question: {model.ExamAndTestQuestionContent}"
                    };
                }

                // Map ExamAndTestQuestionCreateModel to ExamAndTestQuestion entity
                var newQuestion = _mapper.Map<ExamAndTestQuestions>(model);
                newQuestion.ExamAndTestQuestionCode = $"MCH{_context.ExamAndTestQuestionss.Count() + 1:000}";
                await _context.AddAsync(newQuestion);
                newQuestion.CreatedDate = DateTime.Now;
                await _context.SaveChangesAsync();

                // Map and add answers to the question
                if (model.ExamAndTestAnswerCreateModels != null && model.ExamAndTestAnswerCreateModels.Any())
                {
                    foreach (var answerModel in model.ExamAndTestAnswerCreateModels)
                    {
                        var answer = new ExamAndTestAnswers
                        {
                            AnswerContent = answerModel.AnswerContent,
                            isAnswer = answerModel.isAnswer,
                            EaTQuestionId = newQuestion.EaTQuestionId
                        };

                        // Add answer to the context
                        await _context.ExamAndTestAnswerss.AddAsync(answer);
                    }
                }
            }
            await _context.SaveChangesAsync();
            return new APIResponse
            {
                Success = true,
                Message = "Questions created successfully."
            };
        }
        public async Task<APIResponse> CreateQuestion(ExamAndTestQuestionCreateModel model)
        {
            var user = await _context.Users.Include(role => role.UserRole)
              .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
            if(!user.UserRole.ExamsAndTestsCreateNew) {
                return new APIResponse
                {
                    Success = false,
                    Message = "User dont have permission."
                };
            }
            // Check if SubjectId and FacultyId exist in the database
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.SubjectId.Equals(model.SubjectId));
            var faculty = await _context.Facultys.FirstOrDefaultAsync(f => f.FacultyId.Equals(model.FacultyId));

            if (subject == null)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Invalid SubjectId"
                };
            }

            if (faculty == null)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Invalid FacultyId"
                };
            }

            // Map ExamAndTestQuestionCreateModel to ExamAndTestQuestion entity
            var newQuestion = _mapper.Map<ExamAndTestQuestions>(model);
            newQuestion.ExamAndTestQuestionCode = $"MCH{_context.ExamAndTestQuestionss.Count() + 1:000}";
            newQuestion.CreatedDate = DateTime.Now;
            await _context.AddAsync(newQuestion);
            await _context.SaveChangesAsync();
            // Map and add answers to the question
            if (model.ExamAndTestAnswerCreateModels != null && model.ExamAndTestAnswerCreateModels.Any())
            {
                foreach (var answerModel in model.ExamAndTestAnswerCreateModels)
                {
                    var answer = new ExamAndTestAnswers
                    {
                        AnswerContent = answerModel.AnswerContent,
                        isAnswer = answerModel.isAnswer,
                        EaTQuestionId = newQuestion.EaTQuestionId
                    };

                    // Add answer to the context
                    await _context.ExamAndTestAnswerss.AddAsync(answer);
                }

                await _context.SaveChangesAsync();
            }
            return new APIResponse
            {
                Success = true,
                Message = "Created success."
            };
        }     
        #endregion
        //PUT
        #region
        public async Task<APIResponse> UpdateQuestion(string id, ExamAndTestQuestionUpdateModel model)
        {
            var user = await _context.Users.Include(role => role.UserRole)
              .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
            if (!user.UserRole.ExamsAndTestsEdit)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "User dont have permission."
                };
            }
            var question = await _context.ExamAndTestQuestionss.FindAsync(int.Parse(id));
            if (question == null)
            {
                return new APIResponse { Success = false, Message = "Question not found" };
            }
            #region
            foreach (var property in typeof(ExamAndTestQuestionUpdateModel).GetProperties())
            {
                var modelValue = property.GetValue(model);
                if (modelValue != null)
                {
                    var questionProperty = typeof(ExamAndTestQuestions).GetProperty(property.Name);
                    if (questionProperty != null)
                    {
                        questionProperty.SetValue(question, modelValue);
                    }
                }
            }
            #endregion
            question.LastModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return new APIResponse { Success = true, Message = "Question updated successfully" };
        }
        #endregion
        //DELETE
        #region
        public async Task<APIResponse> DeleteQuestion(string id)
        {
            var user = await _context.Users.Include(role => role.UserRole)
              .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
            if (!user.UserRole.ExamsAndTestsDelete)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "User dont have permission."
                };
            }
            var question = await _context.ExamAndTestQuestionss.FindAsync(int.Parse(id));
            if (question == null)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Question not found."
                };
            }
            var allAnswer = await _context.ExamAndTestAnswerss
                .Where(q => q.EaTQuestionId.Equals(question.EaTQuestionId))
                .ToListAsync();
            _context.ExamAndTestAnswerss.RemoveRange(allAnswer);
            _context.ExamAndTestQuestionss.Remove(question);
            await _context.SaveChangesAsync();
            return new APIResponse
            {
                Success = false,
                Message = "Deleted."
            };
        }      
        #endregion
    }
}
