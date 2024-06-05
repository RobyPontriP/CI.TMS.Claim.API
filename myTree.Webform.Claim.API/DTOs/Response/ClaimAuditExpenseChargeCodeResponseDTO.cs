namespace CI.TMS.Claim.API.DTOs.Response
{
    public class ClaimAuditExpenseChargeCodeResponseDTO
    {
        public string RowValue { get; set; }
        public string? CostCenterName { get; set; }
        public string? WorkOrderName { get; set; }
        public string? EntityName { get; set; }
        public string? LegalEntityId { get; set; }
        public decimal? Percentage { get; set; }
        public decimal? Amount { get; set; }
        public string Remarks { get; set; }
    }
}
