using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class TravelAuthorizationItineraryResponseDTO
    {
        public int Id { get; set; }
        public string? TAId { get; set; }
        public int? AirportIdFrom { get; set; }
        public string? AirportFrom { get; set; }
        public string? AirportOtherFrom { get; set; }
        public string? CityFrom { get; set; }
        public string? IataFrom { get; set; }
        public string? NameFrom { get; set; }
        public string? CountryFrom { get; set; }
        public string? CityTo { get; set; }
        public string? IataTo { get; set; }
        public string? NameTo { get; set; }
        public string? CountryTo { get; set; }
        public int? AirportIdTo { get; set; }
        public string? AirportTo { get; set; }
        public string? AirportOtherTo { get; set; }
        public string? ClassOfTravel { get; set; }
        public DateTime? TravelDate { get; set; }
        public string? TravelDateLabel { get; set; }
        public TimeSpan? TravelTime { get; set; }
        public int? TimeType { get; set; }
        public string? TimeTypeLabel { get; set; }
        public string? Remarks { get; set; }
        public bool? IsActive { get; set; }
        public string? TravelType { get; set; }
        public byte? IsTicketRequired { get; set; }

    }
}
