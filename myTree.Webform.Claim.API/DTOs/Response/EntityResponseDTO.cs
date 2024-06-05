using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class EntityResponseDTO
    {
        public string Id { get; set; }
        public string CostCenterId { get; set; }
        public string Name { get; set; }
        public string LegalEntityId { get; set; }
        public string RegionId { get; set; }
        public string CountryId { get; set; }
        public string IsActive { get; set; }
    }
}
