using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class TravelAuthorizationTripDateResponseDTO
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
      
    }
}
