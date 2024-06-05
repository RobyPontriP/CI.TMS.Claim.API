using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("vwPopUpTravelAuthorization")]
    public class TravelAuthorizationPopUp
    {
        [Key]
        [Column("TAID")]
        public string? TAId { get; set; }

        [Column("TRAVELLER")]
        public string? Traveler { get; set; }
        [Column("INITIATOR")]
        public string? Initiator { get; set; }
        [Column("CREATED_BY")]
        public string? CreatedBy { get; set; }
        [Column("TRAVELER_ID")]
        public string? TravelerId { get; set; }

        [Column("DESTINATION")]
        public string? Destination { get; set; }
        [Column("TravelOfficeId")]
        public string? TravelOfficeId { get; set; }
    }
}
