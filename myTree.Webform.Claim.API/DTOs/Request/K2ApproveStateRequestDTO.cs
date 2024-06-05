namespace CI.TMS.Claim.API.DTOs.Request
{
    public partial class K2ApproveStateRequestDTO
    {
        public Guid? Id { get; set; }
        public string? RelevantId { get; set; }
        public string? Module { get; set; }
        public string? Username { get; set; }
        public string? ActivityId { get; set; }
        public int? State { get; set; }
    }
}
