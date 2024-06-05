using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("vwCIF_COST_CENTERS")]
    public class CostCenter
    {
        [Column("CODES")]
        public string? Id { get; set; }
        [Column("DESCRIPTION")]
        public string Name { get; set; }
        [Column("EMP_STATUS")]
        public string Status { get; set; }
        [Column("BUDGET_HOLDER_ID")]
        public string? BudgetHolderId { get; set; }
        [Column("EMP_USER_ID")]
        public string? BudgetHolderUserId { get; set; }
    }
}
