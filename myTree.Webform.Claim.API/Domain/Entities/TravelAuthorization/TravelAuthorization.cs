using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("TravelAuthorization")]
    public class TravelAuthorization
    {
        [Key]
        [Column("id")]
        public string TAId { get; set; }
        [Column("travel_office_id")]
        public string? TravelOfficeId { get; set; }
        [Column("created_by")]
        public string? CreatedBy { get; set; }
        [Column("is_already_have_report")]
        public bool? IsAlreadyHaveTripReport { get; set; }
        [Column("is_already_have_expense_claim")]
        public bool? IsAlreadyHaveExpenseClaim { get; set; }
        [Column("is_already_have_perdiem_claim")]
        public bool? IsAlreadyHavePerdiemClaim { get; set; }
        [Column("advanced_required_other_currency_id")]
        public string? AdvancedRequiredOtherCurrencyId { get; set; }
        [Column("advanced_required_other_currency_price")]
        public double? AdvancedRequiredOtherCurrencyPrice { get; set; }

        [Column("amount_charge_other_currency_id")]
        public string? AmountChargeOtherCurrencyId { get; set; }
        [Column("amount_charge_usd_price")]
        public double? AmountChargeUsdPrice { get; set; }
        [Column("advanced_required_date")]
        public DateTime? AdvanceRequiredDate { get; set; }
        [Column("notes")]
        public string? Notes { get; set; }
        [Column("category_id")]
        public string? TravelCategory { get; set; }
        [Column("amount_travel_kit")]
        public double? AmountTravel { get; set; }

        [Column("IsHaveJournalProcessed")]
        public bool? IsHaveJournalProcessed { get; set; }

    }
}
