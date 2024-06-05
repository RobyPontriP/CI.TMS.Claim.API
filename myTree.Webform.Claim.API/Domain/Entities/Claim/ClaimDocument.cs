using CI.TMS.Claim.API.DTOs.Response;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("ClaimDocument")]
    public class ClaimDocument : BaseEntity<Guid>
    {
        [Key]
        public override Guid Id { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public override bool IsActive { get; set; }
        public override DateTime? CreatedAt { get; set; }
        public override string CreatedBy { get; set; }
        public override DateTime? UpdatedAt { get; set; }
        public override string UpdatedBy { get; set; }
    }
}
