namespace CI.TMS.Claim.API.DTOs.Response
{
    public class ClaimDataResponseDTO
    {
        public Guid Id { get; set; }
        public string TAId { get; set; }
        public string TACode { get; set; }
        public string TravelerType { get; set; }
        public bool? IsAlreadyHaveTripReport { get; set; }
        public string TravelerName { get; set; }
        public string TravelerGender { get; set; }
        public string TravelerDutyPost { get; set; }
        public string TravelerWorkingLocation { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal TotalPerdiemClaim{ get; set; }
        public decimal TotalExpenseClaim { get; set; }
        public decimal TotalTEC { get; set; }
        public double AdvanceAmount { get; set; }
        public string SystemCode { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string ParticipantLetter { get; set; }
        public string DetailTraveler { get; set; }
        public string TravelOfficeId { get; set; }
        public Guid ClaimConditionId { get; set; }
        public string? TravelOfficeName { get; set; }
        public string TravelerUserId { get; set; }
        public string CreatedName { get; set; }
        public string TravelDestination { get; set; }
        
        public string TaxSystem { get; set; }
        public string AparId { get; set; }
        public bool? IsHaveClaim { get; set; }
        public string JournalNo { get; set; }
        public bool? IsTicketRequired { get; set; }
        public string? Period { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string? CountHR { get; set; }
        public string? CountPersonalTravel { get; set; }
        public int? CountSponsorship{ get; set; }
        public double? AmountChargeUsdPrice { get; set; }
        public IList<ClaimCommentResponseDTO> ClaimComment { get; set; }
        public IList<TravelAuthorizationDestinationResponseDTO> Destination { get; set; }
        public IList<TravelAuthorizationCostCenterResponseDTO> ChargeCode { get; set; }
        public IList<ClaimPerdiemResponseDTO> ClaimPerdiem { get; set; }
        public IList<ClaimPerdiemDetailResponseDTO> ClaimPerdiemDetail { get; set; }
        public IList<ClaimPerdiemChargeCodeResponseDTO>? ClaimPerdiemChargeCode { get; set; }
        public IList<ClaimExpenseResponseDTO>? ClaimExpense { get; set; }
        public IList<ClaimExpenseChargeCodeResponseDTO>? ClaimExpenseChargeCode { get; set; }
        public IList<ClaimSupportingDocumentResponseDTO>? ClaimSupportingDocument { get; set; }
        public IList<ClaimBoardingPassDocumentResponseDTO>? ClaimBoardingPassDocument { get; set; }
        public IList<ClaimJournalResponseDTO>? ClaimJournal { get; set; }
        public IList<TravelAuthorizationItineraryResponseDTO>? TravelAuthorizationItinerary { get; set; }
        public IList<TravelAuthorizationExtendedResponseDTO>? TravelAuthorizationExtended { get; set; }
        public List<PerdiemListResponse>? PerdiemRatesList { get; set; }
        public List<PerdiemListResponse>? PerdiemDetailRatesList { get; set; }
        public List<EntityResponseDTO>? AllEntityList { get; set; }
    }
}
