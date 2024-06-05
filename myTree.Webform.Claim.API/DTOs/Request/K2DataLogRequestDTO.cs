namespace CI.TMS.Claim.API.DTOs.Request
{
    public partial class K2DataLogRequestDTO
    {
        public Guid? Id { get; set; }
        public string? RelevantId { get; set; }
        public string? Module { get; set; }
        public string? FieldName { get; set; }
        public string? Value { get; set; }
        public int? SeqNo { get; set; }
    }
}
