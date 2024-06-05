using System.ComponentModel.DataAnnotations;

namespace CI.TMS.Claim.API.Domain.Entities
{
    public class TravelJournalAccountDefault
    {
        [Key]
        public string Id { get; set; }
        public string TravelerType { get; set; }
        public bool IsHaveJournal { get; set; }
        public string Account { get; set; }
        public bool IsActive { get; set; }
    }
}
