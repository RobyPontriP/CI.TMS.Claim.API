namespace CI.TMS.Claim.API.DTOs.Request
{
    public class ClaimExpenseChargeCodeRequestDTO
    {
        public Guid Id { get; set; }
        public string RowId { get; set; }
        public Guid ClaimId { get; set; }
        public Guid ClaimExpenseId { get; set; }
        public string CostCenterId { get; set; }
        public string WorkOrderId { get; set; }
        public string EntityId { get; set; }
        public string LegalEntityId { get; set; }
        public decimal? Percentage { get; set; }
        public decimal? Amount { get; set; }
        public string Remarks { get; set; }
        public int? SeqNo { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
