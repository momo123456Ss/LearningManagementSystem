using AutoMapper;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.LearningStatisticsModel;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;

namespace LearningManagementSystem.Repository
{
    public class LearningStatisticsRepository: InterfaceLearningStatisticsRepository
    {
        private readonly LearningManagementSystemContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LearningStatisticsRepository(LearningManagementSystemContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<APIResponse> GetMinus(string time)
        {
            if (!DateTime.TryParseExact(time, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime start))
            {
                throw new ArgumentException("Invalid start date format. Please use yyyy-MM-dd format.");
            }
            var user = await _context.Users.Include(role => role.UserRole)
               .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
            var flag = await _context.LearningStatisticss
                .Where(u => u.StudentId.Equals(user.UserId))
                .Where(u => u.TimeBegin >= start && u.TimeBegin <= start.Date.AddDays(1).AddTicks(-1))
                .ToListAsync();

            double totalMinutesSum = (double)flag.Sum(x => x.totalMinutes);
            return new APIResponse
            {
                Success = true,
                Message = "OK",
                Data = Math.Round(totalMinutesSum, 2)
            };
        }

        public async Task<APIResponse> SubjectSpendTheMostTimeOn(string academicYear)
        {
            var user = await _context.Users.Include(role => role.UserRole)
               .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
            var flag = await _context.LearningStatisticss
                .Include(c => c.ClassNavigation).ThenInclude(f => f.FacultyNavigation)
                .Include(s => s.SubjectNavigation)
                .Where(u => u.StudentId.Equals(user.UserId))
                .Where(u => u.ClassNavigation.AcademicYear.Equals(academicYear))
                .ToListAsync();
            // Nhóm dữ liệu theo cặp user và subject, sau đó tính tổng của totalMinutes trong mỗi nhóm
            var groupedData = flag.GroupBy(x => new { x.StudentId, x.SubjectNavigation, x.ClassNavigation })
                                  .Select(g => new LearningStatistics
                                  {
                                      StudentId = g.Key.StudentId,
                                      SubjectNavigation = g.Key.SubjectNavigation,
                                      ClassNavigation = g.Key.ClassNavigation,
                                      totalMinutes = Math.Round(g.Sum(x => x.totalMinutes ?? 0),2)
                                  })
                                  .OrderByDescending(x => x.totalMinutes)
                                  .ToList();
            return new APIResponse
            {
                Success = true,
                Message = "OK",
                Data = _mapper.Map<List<LearningStatisticsViewModel>>(groupedData)
            };
        }

        public async Task<APIResponse> TimeBegin(LearningStatisticsModelCreate model)
        {
            var user = await _context.Users.Include(role => role.UserRole)
               .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));

            // Lấy ngày hiện tại
            DateTime currentDate = DateTime.Now;

            // Lấy ngày trong tuần (từ 0 đến 6)
            int dayOfWeekValue = (int)currentDate.DayOfWeek;

            // Chuyển giá trị ngày trong tuần thành chuỗi thứ
            string dayOfWeekString = "";

            switch (dayOfWeekValue)
            {
                case 0:
                    dayOfWeekString = "Chủ Nhật";
                    break;
                case 1:
                    dayOfWeekString = "Thứ Hai";
                    break;
                case 2:
                    dayOfWeekString = "Thứ Ba";
                    break;
                case 3:
                    dayOfWeekString = "Thứ Tư";
                    break;
                case 4:
                    dayOfWeekString = "Thứ Năm";
                    break;
                case 5:
                    dayOfWeekString = "Thứ Sáu";
                    break;
                case 6:
                    dayOfWeekString = "Thứ Bảy";
                    break;
                default:
                    break;
            }

            var newTimeBegin = _mapper.Map<LearningStatistics>(model);
            newTimeBegin.StudentId = user.UserId;
            newTimeBegin.TimeBegin = currentDate;
            newTimeBegin.DayOfWeek = dayOfWeekString;
            await _context.AddAsync(newTimeBegin);
            await _context.SaveChangesAsync();
            return new APIResponse
            {
                Success = true,
                Message = "OK"
            };
        }

        public async Task<APIResponse> TimeEnd(string Id)
        {
            var flag = await _context.LearningStatisticss.FirstOrDefaultAsync(x => x.Id.Equals(int.Parse(Id)));
            DateTime now = DateTime.Now;
            // Tính toán khoảng thời gian
            TimeSpan timeDifference = now.Subtract((DateTime)flag.TimeBegin);
            flag.TimeEnd = now;
            flag.totalMinutes = timeDifference.TotalMinutes;
            await _context.SaveChangesAsync();
            return new APIResponse
            {
                Success = true,
                Message = "OK"
            };
        }
    }
}
