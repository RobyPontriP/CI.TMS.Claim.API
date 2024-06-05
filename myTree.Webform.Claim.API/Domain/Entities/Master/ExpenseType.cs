using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("ExpenseType")]
    public class ExpenseType
    {
        [Key]
        public string ExpenseTypeId { get; set; }
        [Column("ExpenseType")]
        public string ExpenseTypeName { get; set; }
        public string AccountCode { get; set; }
        public string AccountDescription { get; set; }
        public bool IsFinance { get; set; }
        public bool IsBankAccount { get; set; }
        public string RuleId { get; set; }
    }
}
