using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities.Master
{
    [Table("vwAgressoTravelOffice")]
    public class FinanceOffice
    {
        [Key]
        public string Id { get; set; }
        public string TravelOfficeName { get; set; }
        public string  Email { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public bool Status { get; set; }

    }
}
