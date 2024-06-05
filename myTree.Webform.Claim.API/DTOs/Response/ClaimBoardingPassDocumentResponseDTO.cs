namespace CI.TMS.Claim.API.DTOs.Response
{
    public class ClaimBoardingPassDocumentResponseDTO
    {
        public Guid Id { get; set; }
        public Guid ClaimId { get; set; }
        public Guid ClaimDocumentId { get; set; }
        public string? Description { get; set; }
        public int? SeqNo { get; set; }
        public bool? IsActive { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public bool? IsFinance { get; set; }
    }
}
