namespace CI.TMS.Claim.API.DTOs.Request
{
    public class SnValidatorRequestDTO
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ProcessName { get; set; }
        public string Folder { get; set; }
        public string Sn { get; set; }
    }
}
