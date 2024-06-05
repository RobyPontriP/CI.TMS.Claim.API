using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("TravelAuthorizationJournal")]
    public class TravelAuthorizationJournal
    {
        [Key]
        public Guid Id { get; set; }
        public string JournalNumber { get; set; }
        [Column("TravelAuthorizationId")]
        public string TAId { get; set; }
        public Guid JournalDocumentId { get; set; }
        public bool IsActive { get; set; }
    }
}
