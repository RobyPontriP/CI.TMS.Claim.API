using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("VwCURRENCY")]
    public class Currency
    {
        [Key]
        [Column("CURRENCY_CODE")]
        public string CurrencyCode { get; set; }
        [Column("RATE")]
        public decimal Rate { get; set; }
        [Column("OPERATOR")]
        public string Operator { get; set; }
    }
}
