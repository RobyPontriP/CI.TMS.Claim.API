using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities.Master
{
    [Table("vwAgressoTECFinanceOfficer")]
    public class FinanceOfficer
    {
        [Key]
        public Guid Id { get; set; }
        public string EmployeeId { get; set; }
        public string PreferedName { get; set; }
        public string? TravelOfficeId { get; set; }
        public string? FinanceTECId { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public bool Status { get; set; }

    }
}
