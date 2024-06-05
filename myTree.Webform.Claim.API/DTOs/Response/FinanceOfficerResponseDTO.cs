namespace CI.TMS.Claim.API.DTOs.Response
{
    public class FinanceOfficerResponseDTO
    {
        public string EmployeeId { get; set; }
        public string UserId { get; set; }
        public string PreferedName { get; set; }
        public string TravelOfficeId { get; set; }
        public string FinanceTECId { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public bool status { get; set; }
        public string Email { get; set; }
    }
}
