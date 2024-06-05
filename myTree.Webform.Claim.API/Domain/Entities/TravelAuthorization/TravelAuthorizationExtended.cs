using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("TravelAuthorization_extended")]
    public class TravelAuthorizationExtended
    {
        [Key]
        [Column("travel_authorization_id")]
        public string TAId { get; set; }
        [Column("total_all_advanced")]
        public double TotalAdvance { get; set; }
        [Column("system_code")]
        public string? TACode { get; set; }
        [Column("actual_airfare")]
        public double? ActualAirfare { get; set; }
        [Column("other_actual_airfare")]
        public double? OtherActualAirfare { get; set; }
        [Column("actual_airfare_cur")]
        public string? ActualAirfareCur { get; set; }
        [Column("ticket_required")]
        public byte? IsTicketRequired { get; set; }
        [Column("hotel_voucher_required")]
        public byte? IsHotelRequired { get; set; }
        [Column("travel_insurance_required")]
        public byte? IsTravelInsuranceRequired { get; set; }
        [Column("preference_route")]
        public string? Preference { get; set; }

        [Column("flight_ticket")]
        public byte? FlightTicket { get; set; }
        [Column("flexible_travel_dates")]
        public byte? FlexibleTravelDates { get; set; }
        [Column("advance_required")]
        public byte? AdvanceRequired { get; set; }
        [Column("advanced_required_other_currency_id_second")]
        public string? AdvancedRequiredOtherCurrencyIdSecond { get; set; }
        [Column("advanced_required_other_currency_price_second")]
        public double? AdvancedRequiredOtherCurrencyPriceSecond { get; set; }
        [Column("ClearCommitmentId")]
        public int? ClearCommitmentId { get; set; }


    }
}
