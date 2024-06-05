namespace CI.TMS.Claim.API.DTOs.Response
{
    public class AccountCodeResponseDTO
    {
        public string AccountCode { get; set; }
        public string AccountDescription { get; set; }
        public string Type { get; set; }
        public string RuleId { get; set; }
        public string Rule { get; set; }
        public bool Cat1 { get; set; }
        public bool Cat3 { get; set; }
        public bool Cat4 { get; set; }
        public bool Cat5 { get; set; }
        public bool Cat6 { get; set; }
        public bool Cat7 { get; set; }
        public bool Currency { get; set; }
        public bool TaxSystem { get; set; }
        public bool IsBankAccount { get; set; }
    }
}
