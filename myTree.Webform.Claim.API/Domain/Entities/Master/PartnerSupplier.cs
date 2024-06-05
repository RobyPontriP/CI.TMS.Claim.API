using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("vwAgressoPartnerSupplier")]
    public class PartnerSupplier
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TaxSystem { get; set; }
        public string TaxSystemName { get; set; }
    }
}
