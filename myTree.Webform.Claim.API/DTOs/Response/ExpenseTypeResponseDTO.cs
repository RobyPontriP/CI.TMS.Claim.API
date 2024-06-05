using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class ExpenseTypeResponseDTO
    {
        public string ExpenseTypeId { get; set; }
        public string ExpenseTypeName { get; set; }
        public string AccountCode { get; set; }
        public string AccountDescription { get; set; }
        public bool IsFinance { get; set; }
    }
}
