using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("vwAgressoAccountingPeriod")]
    public class AccountingPeriod
    {
        [Key]
        public string Id { get; set; }
        public string Status { get; set; }
        public DateTime LastUpdateAt { get; set; }
    }
}
