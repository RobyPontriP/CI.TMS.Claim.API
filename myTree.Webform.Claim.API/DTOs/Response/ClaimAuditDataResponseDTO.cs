namespace CI.TMS.Claim.API.DTOs.Response
{
    public class ClaimAuditDataResponseDTO
    {
        public string ModuleId { get; set; }
        public string SubModule { get; set; }
        public string RecId { get; set; }
        public string ChangeBy { get; set; }
        public DateTime ChangeTime { get; set; }
        public string ChangeType { get; set; }
        public string FieldName { get; set; }
        public string PreviousValue { get; set; }
        public string NewValue { get; set; }
        public string ReasonOfChange { get; set; }
        public int? ApprovalNo { get; set; }
        public int? SeqNo { get; set; }
    }
}
