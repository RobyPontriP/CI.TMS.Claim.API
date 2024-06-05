namespace CI.TMS.Claim.API.DTOs.Request
{
    public partial class ExecuteWorkflowRequestDTO
    {
        public string Command { get; set; }
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string? Sn { get; set; }
        public string? ActivityId { get; set; }
        public string? Status { get; set; }
        public string? ActionId { get; set; }
        public string? Comment { get; set; }
        public string? Role { get; set; }
    }
}
