using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class CityResponseDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string SubRegionId { get; set; }
        public string RegionId { get; set; }
        public int SeqNo { get; set; }
    }
}
