using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("vwHolidayCalendar")]
    [Keyless]
    public class HolidayCalendar
    {
        public Guid Id { get; set; }
        public string CalDutyId { get; set; }
        public DateTime? CalDate { get; set; }
        public string DayType { get; set; }
        public string DayTypeDescription { get; set; }
        public string DayType2 { get; set; }
        public string DayType2Description { get; set; }

    }
}
