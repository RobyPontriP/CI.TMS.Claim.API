using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class TravelAuthorizationPopUpResponseDTO
    {
        public string TAId { get; set; }
        public string? Traveler { get; set; }
        public string? Initiator { get; set; }
        public string? CreatedBy { get; set; }
        public string? TravelerId { get; set; }
        public string? Destination { get; set; }
        public string? TravelOfficeId { get; set; }

    }
}
