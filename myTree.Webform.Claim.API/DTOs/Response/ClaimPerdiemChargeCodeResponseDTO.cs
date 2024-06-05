namespace CI.TMS.Claim.API.DTOs.Response
{
    public class ClaimPerdiemChargeCodeResponseDTO
    {
        public Guid Id { get; set; }
        public Guid ClaimId { get; set; }
        public Guid ClaimPerdiemId { get; set; }
        public string CostCenterId { get; set; }
        public string CostCenterName { get; set; }
        public string WorkOrderId { get; set; }
        public string WorkOrderName { get; set; }
        public string EntityId { get; set; }
        public string EntityName { get; set; }
        public string LegalEntityId { get; set; }
        public decimal Percentage { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public int SeqNo { get; set; }
        public bool IsActive { get; set; }
        public string BudgetHolderId { get; set; }
        public string BudgetHolderUserId { get; set; }
    }
}
