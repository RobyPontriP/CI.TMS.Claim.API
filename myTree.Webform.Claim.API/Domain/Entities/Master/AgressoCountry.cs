using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("vwAgressoCountry")]
    public class AgressoCountry
    {
        [Key]
        [Column("country_id")]
        public string Id { get; set; }
        [Column("country_name")]
        public string CountryName { get; set; }
        [Column("region_id")]
        public string RegionId { get; set; }
        [Column("sub_region_id")]
        public string SubRegionId { get; set; }
        [Column("sub_region_name")]
        public string SubRegionName { get; set; }
    }
}
