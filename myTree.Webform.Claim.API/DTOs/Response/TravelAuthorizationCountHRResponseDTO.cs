using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class TravelAuthorizationCountHRResponseDTO
    {
        public string CountResult { get; set; }
        public string TAId { get; set; }
      
    }
}
