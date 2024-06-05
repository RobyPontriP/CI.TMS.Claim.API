using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("vwCalendar")]
    [Keyless]
    public class Calendar
    {
        public DateTime DayDate { get; set; }
        public int DayNo { get; set; }
        public string DayDescription { get; set; }
    }
}
