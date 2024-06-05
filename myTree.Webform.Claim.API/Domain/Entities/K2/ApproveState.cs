using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CI.TMS.Claim.API.Domain.Entities.K2
{
    [Table("tblApproveState")]
    public class ApproveState 
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }

        [Column("RelevantId")]
        public string? RelevantId { get; set; }

        [Column("Module")]
        public string? Module { get; set; }

        [Column("Username")]
        public string? Username { get; set; }

        [Column("ActivityId")]
        public string? ActivityId { get; set; }

        [Column("State")]
        public int? State { get; set; }
    }
}
