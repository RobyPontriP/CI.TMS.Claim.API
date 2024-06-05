using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class TravelAuthorizationExtendedResponseDTO
    {
        public double TotalAdvance { get; set; }
        public string? TACode { get; set; }
        public double? ActualAirfare { get; set; }
        public double? OtherActualAirfare { get; set; }
        public string? ActualAirfareCur { get; set; }
        public byte? IsTicketRequired { get; set; }
        public byte? IsHotelRequired { get; set; }
        public byte? IsTravelInsuranceRequired { get; set; }
        public string? Preference { get; set; }
        public string? AdditionalInfo { get; set; }
        public byte? FlightTicket { get; set; }
        public byte? FlexibleTravelDates { get; set; }
        public byte? AdvanceRequired { get; set; }
        public string? AdvancedRequiredOtherCurrencyIdSecond { get; set; }
        public double? AdvancedRequiredOtherCurrencyPriceSecond { get; set; }
        public string? AdvancedRequiredOtherCurrencyId { get; set; }
        public double? AdvancedRequiredOtherCurrencyPrice { get; set; }
        public DateTime? AdvanceRequiredDate { get; set; }
        public bool? IsHaveJournalProcessed { get; set; }
    }
}
