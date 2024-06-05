namespace CI.TMS.Claim.API.DTOs.Response
{
    public class ClaimDocumentResponseDTO
    {
        public Guid? Id { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
    }
}
