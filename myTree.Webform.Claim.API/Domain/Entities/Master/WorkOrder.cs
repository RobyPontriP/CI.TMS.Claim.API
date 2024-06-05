using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("vwdbMasterData_WorkOrder")]
    public class WorkOrder
    {
        [Column("Id")]
        public string? Id { get; set; }
        [Column("Description")]
        public string? Name { get; set; }
        [Column("CostCenterId")]
        public string? CostCenterId { get; set; }
        [Column("CostCenterName")]
        public string? CostCenterName { get; set; }
        [Column("BudgetHolderId")]
        public string? BudgetHolderId { get; set; }
        [Column("BudgetHolderName")]
        public string? BudgetHolderName { get; set; }
        [Column("ProjectId")]
        public string? ProjectId { get; set; }
        [Column("IsActive")]
        public bool IsActive { get; set; }
        [Column("CreatedBy")]
        public string? CreatedBy { get; set; }
        [Column("START_DATE")]
        public DateTime? StartDate { get; set; }
        [Column("END_DATE")]
        public DateTime? EndDate { get; set; }
    }
}
