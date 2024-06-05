using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("vwCIF_DUTYPOST")]
    [Keyless]
    public class DutyPost
    {
        [Column("DUTYPT_CODE")]
        public string? TravelerDutyPostCode { get; set; }
        [Column("DUTYPT_NAME")]
        public string? TravelerDutyPostName { get; set; }
    }
}
