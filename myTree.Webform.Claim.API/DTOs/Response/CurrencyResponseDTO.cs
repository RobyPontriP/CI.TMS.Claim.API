using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class CurrencyResponseDTO
    {
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
        public string Operator { get; set; }
    }
}
