using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("vwAgressoDutyPost")]
    public class AgressoDutyPost
    {
        [Key]
        [Column("dutypost_id")]
        public string Id { get; set; }
        [Column("dutypost_name")]
        public string DutyPosName { get; set; }
        [Column("country_id")]
        public string CountryId { get; set; }
        [Column("country_name")]
        public string CountryName { get; set; }
    }
}
