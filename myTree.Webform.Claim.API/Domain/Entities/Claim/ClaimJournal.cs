using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("ClaimJournal")]
    public class ClaimJournal
    {
        [Key]
        public  Guid Id { get; set; }
        public Guid ClaimId { get; set; }
        public string? JournalNumber { get; set; }
        public string? Account { get; set; }
        public string? Text { get; set; }
        public string? Cat1 { get; set; }
        public string? Cat3 { get; set; }
        public string? Cat4 { get; set; }
        public string? Cat5 { get; set; }
        public string? Cat6 { get; set; }
        public string? Cat7 { get; set; }
        public string? TaxSystem { get; set; }
        public string? Currency { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AmountUsd { get; set; }
        public string? AparId { get; set; }
        public string? AparName { get; set; }
        public bool IsAdd { get; set; }
        public string? CostCenterId { get; set; }
        public string? WorkOrderId { get; set; }
        public string? EntityId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }
}
