namespace CI.TMS.Claim.API.DTOs.Request
{
    public class OCSMFLQueueRequestDTO
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
