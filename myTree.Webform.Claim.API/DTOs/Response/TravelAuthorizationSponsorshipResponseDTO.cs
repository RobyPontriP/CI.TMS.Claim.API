using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class TravelAuthorizationSponsorshipResponseDTO
    {

        public int? Id { get; set; }
        public string? InstitutionName { get; set; }
    }
}
