using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("VWCIF_CITY")]
    public class City
    {
        [Key]
        [Column("city_id")]
        public string Id { get; set; }
        [Column("city_name")]
        public string Name { get; set; }
        [Column("country_id")]
        public string CountryId { get; set; }
        [Column("country_name")]
        public string CountryName { get; set; }
        [Column("date_from")]
        public DateTime DateFrom { get; set; }
        [Column("date_to")]
        public DateTime DateTo { get; set; }
        [Column("seqno")]
        public int SeqNo { get; set; }
    }
}
