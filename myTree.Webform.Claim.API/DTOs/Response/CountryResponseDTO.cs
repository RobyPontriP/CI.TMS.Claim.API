using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class CountryResponseDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CouCode { get; set; }
        public string RegionName { get; set; }
        public string SubRegionName { get; set; }
    }
}
