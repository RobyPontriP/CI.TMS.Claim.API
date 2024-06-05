using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("ClaimPerdiemChargeCode")]
    public class ClaimPerdiemChargeCode : BaseEntity<Guid>
    {
        [Key]
        public override Guid Id { get; set; }
        public Guid ClaimId { get; set; }
        public Guid ClaimPerdiemId { get; set; }
        public string CostCenterId { get; set; }
        public string WorkOrderId { get; set; }
        public string EntityId { get; set; }
        public string LegalEntityId { get; set; }
        public decimal Percentage { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public int SeqNo { get; set; }
        public override bool IsActive { get; set; }
        public override DateTime? CreatedAt { get; set; }
        public override string CreatedBy { get; set; }
        public override DateTime? UpdatedAt { get; set; }
        public override string UpdatedBy { get; set; }
    }
}
