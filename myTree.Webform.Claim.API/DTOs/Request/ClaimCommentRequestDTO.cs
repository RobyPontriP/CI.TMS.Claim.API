using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Request
{
    public class ClaimCommentRequestDTO
    {
        public Guid Id { get; set; }
        public Guid? ClaimId { get; set; }
        public string? Name { get; set; }
        public DateTime? Date { get; set; }
        public string? ActionTaken { get; set; }
        public string? Role { get; set; }
        public string? Comment { get; set; }
        public int? ActivityId { get; set; }
        public string? Sn { get; set; }
        public Guid? DocumentId { get; set; }
    }
}
