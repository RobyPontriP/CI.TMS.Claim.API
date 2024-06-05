using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class TravelAuthorizationCostCenterResponseDTO
    {
        public string? Id { get; set; }
        public double? Percentage { get; set; }
        public string? Remarks { get; set; }
        public string? TAId { get; set; }
        public string? CostCenterId { get; set; }
        public string? CostCenterName { get; set; }
        public string? WorkOrderId { get; set; }
        public string? WorkOrderName { get; set; }
        public string? EntityId { get; set; }
        public string? EntityName { get; set; }
        public double? CostCenterAmount { get; set; }
        public double? CostCenterAmounUSD { get; set; }
        public string? LegalEntityId { get; set; }
        public string? ControlAccount { get; set; }
        public int? SeqNo { get; set; }
    }
}
