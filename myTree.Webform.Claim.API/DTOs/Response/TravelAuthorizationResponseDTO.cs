using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class TravelAuthorizationResponseDTO
    {
        public string? TAId { get; set; }
        public string? TravelOfficeId { get; set; }
        public string? TravelOfficeName { get; set; }
        public string? SystemCode { get; set; }
    }
}
