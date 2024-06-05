namespace CI.TMS.Claim.API.DTOs.Request
{
    public partial class K2ActivityUserListRequestDTO
    {
        public long? Id { get; set; }
        public string? RelevantId { get; set; }
        public string? Folder { get; set; }
        public string? ProcessName { get; set; }
        public string? ActivityName { get; set; }
        public int? ActivityId { get; set; }
        public string? ActivityUser { get; set; }
        public int? SeqNo { get; set; }
    }
}
