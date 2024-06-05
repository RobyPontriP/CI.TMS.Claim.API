namespace CI.TMS.Claim.API.DTOs.Response
{
    public class ClaimAuditPerdiemResponseDTO
    {
        public string RowValue { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string CountryName { get; set; }
        public string? CityName { get; set; }
        public string? CityOther { get; set; }
        public bool? B { get; set; }
        public bool? L { get; set; }
        public bool? D { get; set; }
        public bool? I { get; set; }
        public bool? F { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? Currency { get; set; }

    }
}
