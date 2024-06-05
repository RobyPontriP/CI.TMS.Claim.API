using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Security.Principal;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class K2BudgetHolderResponseDTO
    {
        public string UserId { get; set; }
        public Guid ClaimId { get; set; }
        public string EmpStatus { get; set; }

    }
}
