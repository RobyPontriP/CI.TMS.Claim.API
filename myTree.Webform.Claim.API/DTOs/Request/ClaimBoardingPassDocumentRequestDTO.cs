using CI.TMS.Claim.API.DTOs.Response;

namespace CI.TMS.Claim.API.DTOs.Request
{
    public class ClaimBoardingPassDocumentRequestDTO
    {
        public Guid Id { get; set; }
        public Guid ClaimId { get; set; }
        public Guid ClaimDocumentId { get; set; }
        public DateTime DocumentDate { get; set; }
        public string Description { get; set; }
        public Guid RelatedId { get; set; }
        public int SeqNo { get; set; }
        public bool IsActive { get; set; }
        public bool? IsFinance { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
