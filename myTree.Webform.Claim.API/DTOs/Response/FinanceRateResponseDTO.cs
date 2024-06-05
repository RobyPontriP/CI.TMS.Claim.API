using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class FinanceRateResponseDTO
    {
        public string CurrencyId { get; set; }
        public decimal? ExchangeRate { get; set; }
        public string Operator { get; set; } = "*";
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string RateOperator { get; set; }
    }
}
