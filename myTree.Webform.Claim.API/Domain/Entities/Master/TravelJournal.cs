using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("vwTravelJournal")]
    [Keyless]
    public class TravelJournal
    {
        [Column("TravelAuthorizationId")]
        public string TAId { get; set; }
        public string JournalNumber { get; set; }
        [Column("account")]
        public string Account { get; set; }
        [Column("dim_1")]
        public string Cat1 { get; set; }
        [Column("dim_3")]
        public string Cat3 { get; set; }
        [Column("dim_4")]
        public string Cat4 { get; set; }
        [Column("dim_5")]
        public string Cat5 { get; set; }
        [Column("dim_6")]
        public string Cat6 { get; set; }
        [Column("dim_7")]
        public string Cat7 { get; set; }

        [Column("currency")]
        public string Currency { get; set; }
        [Column("cur_amount")]
        public decimal Amount { get; set; }
        [Column("amount")]
        public decimal AmountUsd { get; set; }
        [Column("tax_system")]
        public string TaxSystem { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("apar_id")]
        public string AparId { get; set; }
    }
}
