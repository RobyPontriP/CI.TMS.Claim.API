using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("vwMFLegalEntity")]
    public class LegalEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TaxSystem { get; set; }
        public string Status { get; set; }
    }
}
