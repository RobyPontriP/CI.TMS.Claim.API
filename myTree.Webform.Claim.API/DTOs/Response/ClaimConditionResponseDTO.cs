using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class ClaimConditionResponseDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

    }
}
