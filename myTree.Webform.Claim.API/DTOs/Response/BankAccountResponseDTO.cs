using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    public class BankAccountDTO
    {
        public List<BankAccountResponseDTO> ListBankAccount { get; set; }
        public List<EntityBankAccountResponseDTO> ListEntityBankAccount { get; set; }

    }
    public class BankAccountResponseDTO
    {
        public string AccountCode { get; set; }
        public string BankAccountCode { get; set; }
        public string BankAccountName { get; set; }
        public string LegalEntityId{ get; set; }
        public string EntityId { get; set; }
        public string EntityName { get; set; }
        public string Currency { get; set; }
        public bool Status { get; set; }
    }

    public class EntityBankAccountResponseDTO
    {
        public string Account { get; set; }
        public string BankAccountCode { get; set; }
        public string EntityId { get; set; }
        public string EntityName { get; set; }
        public string EntityDescription { get; set; }
    }
}
