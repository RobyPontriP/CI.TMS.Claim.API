using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class CostCenterResponseDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }
}
