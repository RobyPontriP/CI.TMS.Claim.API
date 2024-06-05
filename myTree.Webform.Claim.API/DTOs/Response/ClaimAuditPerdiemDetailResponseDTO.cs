namespace CI.TMS.Claim.API.DTOs.Response
{
    public class ClaimAuditPerdiemDetailResponseDTO
    {

        public string? RowValue { get; set; }
        public string? Date { get; set; }
        public string? CountryName { get; set; }
        public string? CityName { get; set; }
        public string? CityOther { get; set; }
        public bool? B { get; set; }
        public bool? L { get; set; }
        public bool? D { get; set; }
        public bool? I { get; set; }
        public bool? F { get; set; }
        public bool? BFinance { get; set; }
        public bool? LFinance { get; set; }
        public bool? DFinance { get; set; }
        public bool? IFinance { get; set; }
        public bool? FFinance { get; set; }
        public decimal? Amount { get; set; }
        public decimal? BAmount { get; set; }
        public decimal? LAmount { get; set; }
        public decimal? DAmount { get; set; }
        public decimal? IAmount { get; set; }
        public decimal? AmountFinance { get; set; }
        public decimal? BFinanceAmount { get; set; }
        public decimal? LFinanceAmount { get; set; }
        public decimal? DFinanceAmount { get; set; }
        public decimal? IFinanceAmount { get; set; }
        public string? Currency { get; set; }
    }
}
