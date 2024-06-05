using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class TravelAuthorizationCountSponsorshipResponseDTO
    {
        public int CountResult { get; set; }
        public string? TAId { get; set; }
    }
}
