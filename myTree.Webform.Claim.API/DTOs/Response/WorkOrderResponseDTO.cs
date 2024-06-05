using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class WorkOrderResponseDTO
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string CostCenterId { get; set; }
        public string CostCenterName { get; set; }
        public string BudgetHolderId { get; set; }
        public string BudgetHolderName { get; set; }
        public string ProjectId { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
