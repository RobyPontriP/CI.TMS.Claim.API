using CI.TMS.Claim.API.DTOs.Response;

namespace CI.TMS.Claim.API.DTOs.Request
{
    public class ClaimDocumentRequestDTO
    {
        public Guid Id { get; set; }
        public string FileName  { get; set; }
        public string FileUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
   
}
