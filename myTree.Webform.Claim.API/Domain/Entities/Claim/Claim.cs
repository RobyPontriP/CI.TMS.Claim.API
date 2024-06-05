using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("Claim")]
    public class Claim: BaseEntity<Guid>
    {
        [Key]
        public override Guid Id { get; set; }
        public string? TAId { get; set; }
        
        public decimal? TotalPerdiemClaim { get; set; }
        
        public decimal? TotalExpenseClaim { get; set; }
        
        public decimal? TotalTEC { get; set; }
        
        public decimal? AdvanceAmount { get; set; }
        public string? SystemCode { get; set; }
        public string? StatusId { get; set; }
        public override bool IsActive { get; set; }
        public Guid ClaimConditionId { get; set; }
        public bool? IsHaveClaim { get; set; }
        public override DateTime? CreatedAt { get; set; }
        public override string? CreatedBy { get; set; }
        public override DateTime? UpdatedAt { get; set; }
        public override string? UpdatedBy { get; set; }
        public string? TravelOfficeId { get; set; }
        public string? JournalNo { get; set; }
        public string? Period { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal? AmountChargeToPersonal { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
