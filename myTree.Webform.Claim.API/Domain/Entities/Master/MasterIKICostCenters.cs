using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("MasterIkiCostCenter")]
    [Keyless]
    public class MasterIKICostCenters
    {   
        [Column("ProjectId")]
        public string ProjectId { get; set; }
        [Column("PerdiemType")]
        public string PerdiemType { get; set; }
       
    }
}
