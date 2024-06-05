namespace CI.TMS.Claim.API.DTOs.Request
{
    public class ReportTECRequestDTO
    {
        public string Traveler { get; set; }
        public string Destination { get; set; }
        public string Date { get; set; }
        public string SystemCode { get; set; }
        public string Period { get; set; }
        public string TransactionNo { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string JournalNo { get; set; }
        public List<ClaimJournalRequestDTO>? ClaimJournal { get; set; }

    }
}
