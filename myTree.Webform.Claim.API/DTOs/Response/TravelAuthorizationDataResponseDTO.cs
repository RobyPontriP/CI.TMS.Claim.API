namespace CI.TMS.Claim.API.DTOs.Response
{
    public class TravelAuthorizationDataResponseDTO
    {
        public string TAId { get; set; }
        public IEnumerable<TravelAuthorizationDestinationResponseDTO> Destination { get; set; }
        public IEnumerable<TravelAuthorizationCostCenterResponseDTO> ChargeCode { get; set; }
        public bool? IsAlreadyHaveTripReport { get; set; }
        public string TravelerType { get; set; }
        public string TravelerName { get; set; }
        public string TravelOfficeId { get; set; }
        public string TravelOfficeName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal TotalAdvance { get; set; }
        public string? TravelCategory { get; set; }
        public double? AmountTravel { get; set; }
        public double? AmountChargeUsdPrice { get; set; }
    }
}
