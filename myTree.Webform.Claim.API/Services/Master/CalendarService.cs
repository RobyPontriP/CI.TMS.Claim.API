using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using System.Linq.Expressions;
using System.Linq;
using CI.TMS.Claim.API.Domain.Entities;

namespace CI.TMS.Claim.API.Services
{
    public class CalendarService : BaseService
    {
        private readonly Dictionary<string, string> DutyMapping = new Dictionary<string, string>() {
            { "ICRAF", "KENYA" },
            { "CIFOR", "INDON" },
            { "GERMANY", "INDON" },
        };
        private string DutySearch = string.Empty;
        public CalendarService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<BaseService> log)
            : base(context, httpContextAccessor, log)
        {
        }

        public async Task<DateTime?> GetDueDate(DateTime date, int days, string legalEntityId = "")
        {
            try
            {
                if (string.IsNullOrEmpty(legalEntityId))
                {
                    DutySearch = "INDON";
                }
                else
                {
                    DutySearch = DutyMapping[legalEntityId];

                }
                var result = new DateTime?();
                if (days < 0)
                    result = GetPreviousDate(date, days);
                else if (days > 0)
                    result = GetNextDate(date, days);
                else
                    result = null;

                return result;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        private DateTime GetNextDate(DateTime date, int days)
        {
            var y = (from c in context.Calendar
                     where c.DayDate.Date > date.Date && c.DayNo != 7 && c.DayNo != 1
                        && !context.HolidayCalendar.Any(h => (h.CalDate == c.DayDate) && h.DayType == "STD" && h.CalDutyId.Contains(DutySearch))
                     orderby c.DayDate
                     select c.DayDate).Take(days).ToList();
            var result = y.OrderByDescending(y => y).FirstOrDefault();

            return result;
        }

        private DateTime GetPreviousDate(DateTime date, int days)
        {
            var y = (from c in context.Calendar
                     where c.DayDate.Date < date.Date && c.DayNo != 7 && c.DayNo != 1
                        && !context.HolidayCalendar.Any(h => (h.CalDate == c.DayDate) && h.DayType == "STD" && h.CalDutyId.Contains(DutySearch))
                     orderby c.DayDate descending
                     select c.DayDate).Take(Math.Abs(days)).ToList();

            var result = y.OrderBy(y => y).FirstOrDefault();

            return result;
        }
    }
}
