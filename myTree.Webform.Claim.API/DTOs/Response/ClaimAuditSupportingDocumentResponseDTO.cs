namespace CI.TMS.Claim.API.DTOs.Response
{
    public class ClaimAuditSupportingDocumentResponseDTO
    {   
        public string RowValue { get; set; }
        public string DocumentDate { get; set; }
        public string? Description { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
    }
}
