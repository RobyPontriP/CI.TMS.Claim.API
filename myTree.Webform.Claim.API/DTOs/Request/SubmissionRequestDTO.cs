using Microsoft.EntityFrameworkCore;

namespace CI.TMS.Claim.API.DTOs.Request
{
    public class SubmissionRequestDTO
    {
        public Guid Id { get; set; }
        public string Action { get; set; }
        public string TAId { get; set; }

        public decimal TotalPerdiemClaim { get; set; }

        public decimal TotalExpenseClaim { get; set; }

        public decimal AdvanceAmount { get; set; }

        public decimal TotalTEC { get; set; }
        public string StatusId { get; set; }
        public string? UserId { get; set; }
        public string? TravelOfficeId { get; set; }
        public Guid ClaimConditionId { get; set; }
        public string? Source { get; set; }
        public string? ActionName { get; set; }
        public string? Sn { get; set; }
        public string? ActId { get; set; }
        public string? Comment { get; set; }
        public string? Role { get; set; }
        public bool? IsHaveClaim { get; set; }
        public string? Page { get; set; }
        public string? Period { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? DueDate { get; set; }
        public virtual IList<ClaimCommentRequestDTO>? ClaimComment { get; set; }
        public virtual IList<ClaimPerdiemRequestDTO>? ClaimPerdiem { get; set; }
        public virtual IList<ClaimPerdiemDetailRequestDTO>? ClaimPerdiemDetail { get; set; }
        public virtual IList<ClaimPerdiemChargeCodeRequestDTO>? ClaimPerdiemChargeCode { get; set; }
        public virtual IList<ClaimExpenseRequestDTO>? ClaimExpense { get; set; }
        public virtual IList<ClaimExpenseChargeCodeRequestDTO>? ClaimExpenseChargeCode { get; set; }
        public virtual IList<ClaimSupportingDocumentRequestDTO>? ClaimSupportingDocument { get; set; }
        public virtual IList<ClaimBoardingPassDocumentRequestDTO>? ClaimBoardingPassDocument { get; set; }
        public virtual IList<ClaimJournalRequestDTO>? ClaimJournal { get; set; }


    }
}
