using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("TravelAuthorizationSponsorship")]
    public class TravelAuthorizationSponsorship
    {
        [Key]
        public int? Id { get; set; }
        [Column("TravelAuthorizationId")]
        public string? TAId { get; set; }
        [Column("InstitutionName")]
        public string? InstitutionName { get; set; }
        public decimal? TypeValue { get; set; }
    }
}
