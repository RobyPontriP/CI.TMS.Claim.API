namespace CI.TMS.Claim.API.DTOs.Response
{
    public class FinanceOfficeResponseDTO
    {
        public string Id { get; set; }
        public string TravelOfficeName { get; set; }
        public string Email { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public bool Status { get; set; }
    }
}
