namespace CI.TMS.Claim.API.DTOs.Response
{
    public class TravelJournalAccountDefaultResponseDTO
    {
        public string Id { get; set; }
        public string TravelerType { get; set; }
        public bool IsHaveJournal { get; set; }
        public string Account { get; set; }
        public bool IsActive { get; set; }
    }
}
