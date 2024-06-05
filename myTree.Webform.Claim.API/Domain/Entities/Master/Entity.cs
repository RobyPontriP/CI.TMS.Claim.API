using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("vwdbMasterData_Entity_CostC")]
    public class Entity
    {
        [Column("EntityId")]
        public string Id { get; set; }
        [Column("CostCenterId")]
        public string CostCenterId { get; set; }
        [Column("Description")]
        public string Name { get; set; }
        [Column("LegalEntityId")]
        public string LegalEntityId { get; set; }
        [Column("RegionId")]
        public string RegionId { get; set; }
        [Column("CountryId")]
        public string CountryId { get; set; }
        [Column("IsActive")]
        public string IsActive { get; set; }
    }
}
