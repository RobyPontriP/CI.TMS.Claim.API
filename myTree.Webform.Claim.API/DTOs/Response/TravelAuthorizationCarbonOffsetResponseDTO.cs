namespace CI.TMS.Claim.API.DTOs.Response
{
    public class TravelAuthorizationCarbonOffsetResponseDTO
    {
        public string TAId { get; set; }
        public string? TravelCategory { get; set; }
        public double? AmountTravel { get; set; }
        public string JournalAccountCarbon { get; set; }
        public string JournalAccountBalancer { get; set; }
        public string AparId { get; set; }
        public double? Percentage { get; set; }
        public string? CostCenterId { get; set; }
        public string? WorkOrderId { get; set; }
        public string? EntityId { get; set; }
        public double? CostCenterAmount { get; set; }
        public double? CostCenterAmountUSD { get; set; }
        public string? LegalEntityId { get; set; }
    }
}
