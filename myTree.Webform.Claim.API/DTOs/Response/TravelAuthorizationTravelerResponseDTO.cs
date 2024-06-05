using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class TravelAuthorizationTravelerResponseDTO
    {
        public string? Id { get; set; }
        public string? TravelerId { get; set; }
        public string? TravelerName { get; set; }
        public string? TAId { get; set; }
        public string? DirectSpv { get; set; }
        public string? TravelerType { get; set; }
        public string? ParticipantLetter { get; set; }
        public string? DetailTraveler { get; set; }
        public int? Gender { get; set; }
        public string? WorkingLocation { get; set; }
        public string? TravelerDutyPostName { get; set; }
        public string? TaxSystem { get; set; }
        public string? AparId { get; set; }
    }
}
