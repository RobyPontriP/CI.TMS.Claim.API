using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("TravelAuthorizationTraveler_Extended")]
    public class TravelAuthorizationTravelerExtended
    {
        [Key]
        [Column("TravelAuthorizationTraveler_id")]
        public string? TATravelerId { get; set; }
        [Column("traveler_type")]
        public string? TravelerType { get; set; }
        [Column("traveler")]
        public string? DetailTraveler { get; set; }
        [Column("participant_letter")]
        public string? ParticipantLetter { get; set; }
        [Column("working_location")]
        public string? WorkingLocation { get; set; }
        [Column("duty_post_code")]
        public string? TravelerDutyPostCode { get; set; }

    }
}
