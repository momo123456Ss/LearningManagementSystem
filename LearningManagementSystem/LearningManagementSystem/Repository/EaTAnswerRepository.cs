using AutoMapper;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.ExamAndTestQuestionModel;
using LearningManagementSystem.Models.ExamAndTestQuestionModel.AnswerModel;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LearningManagementSystem.Repository
{
    public class EaTAnswerRepository : InterfaceEaTAnswerRepository
    {
        private readonly LearningManagementSystemContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EaTAnswerRepository(LearningManagementSystemContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<APIResponse> AddAnswer(ExamAndTestAnswerAddOrUpdateModel model)
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
            var question = await _context.ExamAndTestQuestionss.FindAsync(model.EaTQuestionId);
            if (question == null)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Question not found."
                };
            }
            question.LastModifiedDate = DateTime.Now;
            var newAnswer = _mapper.Map<ExamAndTestAnswers>(model);
            await _context.AddAsync(newAnswer);
            await _context.SaveChangesAsync();
            return new APIResponse
            {
                Success = true,
                Message = "Created success."
            };
        }
        public async Task<APIResponse> UpdateAnswer(string answerId, ExamAndTestAnswerAddOrUpdateModel model)
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
            var answer = await _context.ExamAndTestAnswerss.FindAsync(int.Parse(answerId));
            if (answer == null)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Answer not found."
                };
            }
            if (model.EaTQuestionId != null)
            {
                var question = await _context.ExamAndTestQuestionss.FindAsync(model.EaTQuestionId);
                if (question == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Question not found."
                    };
                }
                question.LastModifiedDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            var questionUpdate = await _context.ExamAndTestQuestionss.FindAsync(answer.EaTQuestionId);
            questionUpdate.LastModifiedDate = DateTime.Now;
            #region
            foreach (var property in typeof(ExamAndTestAnswerAddOrUpdateModel).GetProperties())
            {
                var modelValue = property.GetValue(model);
                if (modelValue != null)
                {
                    var answerProperty = typeof(ExamAndTestAnswers).GetProperty(property.Name);
                    if (answerProperty != null)
                    {
                        answerProperty.SetValue(answer, modelValue);
                    }
                }
            }
            #endregion
            await _context.SaveChangesAsync();
            return new APIResponse { Success = true, Message = "Answer updated successfully" };
        }
        public async Task<APIResponse> DeleteAnswer(string answerId)
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
            var answer = await _context.ExamAndTestAnswerss.FindAsync(int.Parse(answerId));
            var question = await _context.ExamAndTestQuestionss.FindAsync(answer.EaTQuestionId);
            question.LastModifiedDate = DateTime.Now;
            if (answer == null)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Answer not found."
                };
            }
            _context.ExamAndTestAnswerss.Remove(answer);
            await _context.SaveChangesAsync();
            return new APIResponse { Success = true, Message = "Answer deleted successfully" };
        }
    }
}
