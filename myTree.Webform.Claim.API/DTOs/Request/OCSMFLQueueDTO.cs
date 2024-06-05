using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CI.TMS.Claim.API.DTOs.Request
{
    public class OCSMFLQueueDTO
    {
        public Guid Id { get; set; }

        public string? SourceName { get; set; }

        public string? SourceId { get; set; }

        public string? UserId { get; set; }

        public DateTime? Time { get; set; }

        public DateTime? DueTime { get; set; }

        public string? Status { get; set; }

        public string? Remark { get; set; }

        public string? MyTreeReferenceNo { get; set; }
    }
}
