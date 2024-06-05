namespace CI.TMS.Claim.API.Domain.Entities.K2
{
    public class K2ApproveState
    {
        public Guid Id { get; set; }
        public Guid ModuleId { get; set; }
        public string Module { get; set; }
        public string UserId { get; set; }
        public int ActivityId { get; set; }
        public bool State { get; set; }
    }
}
