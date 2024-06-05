namespace CI.TMS.Claim.API.DTOs.Request
{
    public class ClaimRequestDTO
    {
        public Guid Id { get; set; }
        public string? TAId { get; set; }
        public decimal? TotalPerdiemClaim { get; set; }
        public decimal? TotalExpenseClaim { get; set; }
        public decimal? TotalTEC{ get; set; }
        public decimal? AdvanceAmount { get; set; }
        public decimal? AmountChargeToPersonal { get; set; }
        public string? StatusId { get; set; }
        public string? TravelOfficeId { get; set; }
        public string? SystemCode { get; set; }
        public Guid ClaimConditionId { get; set; }
        public bool? IsHaveClaim { get; set; }
        public string? Period { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
