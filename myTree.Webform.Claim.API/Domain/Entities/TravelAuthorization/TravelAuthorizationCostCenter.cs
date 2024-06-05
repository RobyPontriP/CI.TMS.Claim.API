using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    public class TravelAuthorizationCostCenter
    {
        [Key]
        public string? Id { get; set; }
        [Column("cost_center_price")]
        public double? Percentage { get; set; }
        public string? Remarks { get; set; }
        [Column("travel_authorization_id")]
        public string? TAId { get; set; }
        [Column("costc")]
        public string? CostCenterId { get; set; }
        [Column("cost_center_id")]
        public string? CostCenterName { get; set; }
        [Column("work_order")]
        public string? WorkOrderId { get; set; }
        [Column("t4")]
        public string? WorkOrderName { get; set; }
        [Column("entity_id")]
        public string? EntityId { get; set; }
        [Column("cost_center_amount")]
        public double? CostCenterAmount { get; set; }
        [Column("cost_center_amount_usd")]
        public double? CostCenterAmounUSD { get; set; }
        [Column("legal_entity_id")]
        public string? LegalEntityId { get; set; }
        [Column("control_account")]
        public string? ControlAccount { get; set; }
        [Column("seq_no")]
        public int? SeqNo { get; set; }
    }
}
