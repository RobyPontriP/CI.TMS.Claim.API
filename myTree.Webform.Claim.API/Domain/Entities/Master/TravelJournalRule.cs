using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    public class TravelJournalRule
    {
        [Key]
        [Column("Id")]
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
    }
}
