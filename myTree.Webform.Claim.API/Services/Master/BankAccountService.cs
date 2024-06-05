using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;

namespace CI.TMS.Claim.API.Services
{
    public class BankAccountService : BaseService
    {
        public BankAccountService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<BaseService> log)
          : base(context, httpContextAccessor, log)
        {
        }

        public async Task<BankAccountDTO> Get(Expression<Func<BankAccount, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => x.Status;

                var listEntityBankAccount = await context.BankAccount
                    .Where(predicate)
                    .Select(x => new EntityBankAccountResponseDTO
                    {
                        Account = x.Account,
                        BankAccountCode = x.BankAccountCode,
                        EntityId = x.EntityId,
                        EntityName = x.EntityName ?? "",
                        EntityDescription = x.EntityDescription
                    })
                    .OrderBy(x => x.EntityDescription)
                    .ToListAsync();

                var listBankAccount = await context.BankAccount
                    .Where(predicate)
                    .Select(x => new BankAccountResponseDTO
                    {
                        AccountCode = x.Account,
                        BankAccountCode = x.BankAccountCode,
                        BankAccountName = x.BankAccountName,
                        LegalEntityId = x.LegalEntityId,
                        EntityId = x.EntityId,
                        EntityName = x.EntityName ?? "",
                        Currency = x.Currency,
                    })
                    .OrderBy(x => x.BankAccountCode)
                    .ToListAsync();

                var bankAccount = new BankAccountDTO
                {
                    ListBankAccount = listBankAccount,
                    ListEntityBankAccount = listEntityBankAccount
                };

                return bankAccount;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}
