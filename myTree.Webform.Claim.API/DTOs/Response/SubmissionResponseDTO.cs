namespace CI.TMS.Claim.API.DTOs.Response
{
    public class SubmissionResponseDTO
    {
        public string TAId { get; set; }
        public Guid ClaimId { get; set; }
        public string SystemCode { get; set; }
        public string TravelDestination { get; set; }
        public IList<TravelAuthorizationDestinationResponseDTO> Destination { get; set; }
        public IList<TravelAuthorizationCostCenterResponseDTO> ChargeCode { get; set; }
        public string TravelerType { get; set; }
        public string TravelerName { get; set; }
        public string TravelerUserId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal TotalAdvance { get; set; }
        public decimal TotalPerdiemClaim { get; set; }
        public decimal TotalExpenseClaim { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal TotalTEC { get; set; }
        public string? TravelOfficeId { get; set; }
        
    }
}
