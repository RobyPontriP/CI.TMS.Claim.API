using CI.TMS.Claim.API.DTOs.Response;

namespace CI.TMS.Claim.API.DTOs.Request
{
    public class UploadDocumentRequestDTO   
    {
        public Guid Id { get; set; }
        public Guid ClaimId { get; set; }
        public string? TableName { get; set; }
        public string? FieldName { get; set; }

        public IFormFileWrapperResponseDTO Document { get; set; }
    }
   
}
