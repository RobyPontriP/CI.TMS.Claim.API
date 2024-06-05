namespace CI.TMS.Claim.API.DTOs.Response
{
    public class ClaimResponseDTO
    {
        public Guid Id { get; set; }
        public string? TAId { get; set; }
        public decimal? TotalPerdiemClaim { get; set; }
        public decimal? TotalExpenseClaim { get; set; }
        public decimal? TotalTEC { get; set; }
        public decimal? AdvanceAmount { get; set; }
        public string? SystemCode { get; set; }
        public string? StatusId { get; set; }
        public bool IsActive { get; set; }
        public bool? IsHaveClaim { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public string? TravelOfficeId { get; set; }
        public string? JournalNo { get; set; }
        //public bool? IsTicketRequired { get; set; }
        public Guid ClaimConditionId { get; set; }
        public string? Period { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
