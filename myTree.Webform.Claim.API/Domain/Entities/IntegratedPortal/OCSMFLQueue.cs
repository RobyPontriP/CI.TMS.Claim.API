using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities.IntegratedPortal
{
    [Table("OCSMFLQueue")]
    public class OCSMFLQueue 
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }

        [Column("SourceName")]
        public string? SourceName { get; set; }

        [Column("SourceId")]
        public string? SourceId { get; set; }

        [Column("UserId")]
        public string? UserId { get; set; }

        [Column("Time")]
        public DateTime? Time { get; set; }

        [Column("DueTime")]
        public DateTime? DueTime { get; set; }

        [Column("Status")]
        public string? Status { get; set; }

        [Column("Remark")]
        public string? Remark { get; set; }

        [Column("MyTreeReferenceNo")]
        public string? MyTreeReferenceNo { get; set; }
    }
}
