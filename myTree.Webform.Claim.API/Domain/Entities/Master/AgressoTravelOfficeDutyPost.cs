using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("vwAgressoTravelOfficeDutyPost")]
    public class AgressoTravelOfficeDutyPost
    {
        [Key]
        public Guid Id { get; set; }
        public string DutyPostId { get; set; }
        public string DutyPosName { get; set; }
        public string TravelOfficeId { get; set; }
    }
}
