using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class ClaimTotalResponseDTO
    {
        public Guid Id { get; set; }
        public double TotalPerdiemClaim { get; set; }
        public double TotalExpenseClaim { get; set; }
        public double TotalTEC { get; set; }
        public string SystemCode { get; set; }
        public string TravelOfficeId { get; set; }
        public string StatusId { get; set; }
        public string CreatedBy { get; set; }
        public Guid ClaimConditionId { get; set; }
        public string? Period { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? DueDate { get; set; }
      
    }
}
