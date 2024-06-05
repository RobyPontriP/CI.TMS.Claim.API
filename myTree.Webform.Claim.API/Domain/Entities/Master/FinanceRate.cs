using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("VwAgressoFinanceRate")]
    public class FinanceRate
    {
        [Key]
        public string CurrencyId { get; set; }
        public decimal? ExchangeRate { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string RateOperator { get; set; }
    }
}
