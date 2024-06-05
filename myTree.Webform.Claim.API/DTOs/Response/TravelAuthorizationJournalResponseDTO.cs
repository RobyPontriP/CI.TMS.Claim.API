using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class TravelAuthorizationJournalResponseDTO
    {
        public Guid Id { get; set; }
        public string JournalNumber { get; set; }
        public string TAId { get; set; }
        public Guid JournalDocumentId { get; set; }
        public bool IsActive { get; set; }
    }
}
