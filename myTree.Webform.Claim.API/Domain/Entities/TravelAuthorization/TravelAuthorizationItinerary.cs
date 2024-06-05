using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("TravelAuthorizationItinerary")]
    public class TravelAuthorizationItinerary
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("travel_authorization_id")]
        public string? TAId { get; set; }
        [Column("airport_id_from")]
        public int? AirportIdFrom { get; set; }
        [Column("airport_other_from")]
        public string? AirportOtherFrom { get; set; }
        [Column("airport_id_to")]
        public int? AirportIdTo { get; set; }
        [Column("airport_other_to")]
        public string? AirportOtherTo { get; set; }
        [Column("class_of_travel")]
        public string? ClassOfTravel { get; set; }
        [Column("travel_date")]
        public DateTime? TravelDate { get; set; }
        [Column("tavel_time")]
        public TimeSpan? TravelTime { get; set; }
        [Column("time_type")]
        public int? TimeType { get; set; }
        [Column("remarks")]
        public string? Remarks { get; set; }
        [Column("is_active")]
        public bool? IsActive { get; set; }
        [Column("travel_type")]
        public string? TravelType { get; set; }

    }
}
