using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("ClaimExpense")]
    public class ClaimExpense : BaseEntity<Guid>
    {
        [Key]
        public override Guid Id { get; set; }
        public Guid ClaimId { get; set; }
        public DateTime? ExpenseClaimDate { get; set; }
        public string? CountryId { get; set; }
        public string? CountryName { get; set; }
        public string? CityId { get; set; }
        public string? CityName { get; set; }
        public string? OtherCityLocation { get; set; }
        public string? Expenditure { get; set; }
        public string? ReceiptNo { get; set; }
        public string? CurrencyId { get; set; }
        public string? CurrencyName { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AmountApproval { get; set; }
      
        [Precision(18, 8)]
        public decimal ExchangeRate { get; set; }
        public decimal? USDAmount { get; set; }
        public string? Remarks { get; set; }
        public string? Operator { get; set; }
        public Guid? ClaimDocumentId { get; set; }
        public string? ExpenseTypeId { get; set; }
        public decimal? AmountApprovalUsd { get; set; }
        [Precision(18, 8)]
        public decimal? ExchangeRateApproval { get; set; }
        public string? OperatorApproval { get; set; }
        public string? CommentApproval { get; set; }
        public string? ReasonDisagree { get; set; }
        public string? StatusApproval { get; set; }
        public Guid? DisagreeDocumentId { get; set; }
        public string? CurrencyIdApproval { get; set; }
        public string? CurrencyNameApproval { get; set; }
        public string? ExpenseTypeIdApproval { get; set; }
        public string? Status { get; set; }
        public bool? IsFinance { get; set; }
        public override bool IsActive { get; set; }
        public override DateTime? CreatedAt { get; set; }
        public override string CreatedBy { get; set; }
        public override DateTime? UpdatedAt { get; set; }
        public override string UpdatedBy { get; set; }
    }
}
