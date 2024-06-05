using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class MasterIKICostCentersResponseDTO
    {
        public string? ProjectId { get; set; }
        public string? PerdiemType { get; set; }
    }
}
