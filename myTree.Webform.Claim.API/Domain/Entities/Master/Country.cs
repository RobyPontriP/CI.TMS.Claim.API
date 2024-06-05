using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{

    [Table("vwCIF_COUNTRIES")]
    public class Country
    {
        [Key]
        [Column("COUNTRYID")]
        public string Id { get; set; }
        [Column("COUNTRY_NM")]
        public string Name { get; set; }
        [Column("COU_CODE")]
        public string CouCode { get; set; }
        [Column("REGION_NM")]
        public string RegionName { get; set; }
        [Column("SUB_REGION_NM")]
        public string SubRegionName { get; set; }
    }
}
