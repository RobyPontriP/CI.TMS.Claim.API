using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("vwAgressoBankAccount")]
    [Keyless]
    public class BankAccount
    {
        public string Account { get; set; }
        [Column("BankAccount")]
        public string BankAccountCode { get; set; }
        public string BankAccountName { get; set; }
        public string LegalEntityId{ get; set; }
        public string EntityId { get; set; }
        public string EntityName { get; set; }
        public string EntityDescription { get; set; }
        public string Currency { get; set; }
        public bool Status { get; set; }
    }
}
