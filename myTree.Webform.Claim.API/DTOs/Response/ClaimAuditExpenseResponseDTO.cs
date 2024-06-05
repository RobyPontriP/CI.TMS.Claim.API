using Microsoft.EntityFrameworkCore;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class ClaimAuditExpenseResponseDTO
    {   
        public string RowValue { get; set; }    
        public string ExpenseClaimDate { get; set; }
        public string? CountryName { get; set; }
        public string? CityName { get; set; }
        public string? CityOther{ get; set; }
        public string? Expenditure { get; set; }
        public string? CurrencyName { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AmountApproval { get; set; }
        [Precision(18, 8)]
        public decimal ExchangeRate { get; set; }
        public decimal? USDAmount { get; set; }
        public string? Remarks { get; set; }
        public Guid? ClaimDocumentId { get; set; }
        public string? ExpenseTypeName { get; set; }
        public decimal? AmountApprovalUsd { get; set; }
        [Precision(18, 8)]
        public decimal? ExchangeRateApproval { get; set; }
        public string? CommentApproval { get; set; }
        public string? ReasonDisagree { get; set; }
        public Guid? DisagreeDocumentId { get; set; }
        public string? CurrencyNameApproval { get; set; }
        public string? ExpenseTypeNameApproval { get; set; }
        public string? Status { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public string? FileNameFinance { get; set; }
        public string? FileUrlFinance { get; set; }
        public string? FileNameDisagree { get; set; }
        public string? FileUrlDisagree { get; set; }
        public bool? IsFinance { get; set; }
    }
}
