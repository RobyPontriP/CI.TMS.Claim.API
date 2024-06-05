using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("TravelAuthorizationTraveler")]
    public class TravelAuthorizationTraveler
    {
        [Key]
        public string? Id { get; set; }
        [Column("traveler_id")]
        public string? TravelerId { get; set; }
        [Column("traveler_name")]
        public string? TravelerName { get; set; }
        [Column("travel_authorization_id")]
        public string? TAId { get; set; }
        [Column("direct_supervisor")]
        public string? DirectSpv { get; set; }
        //[Column("traveler_type")]
        //public string TravelerType { get; set; }
    }
}
