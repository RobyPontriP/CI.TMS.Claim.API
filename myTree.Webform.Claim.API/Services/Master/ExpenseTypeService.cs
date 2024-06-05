using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;
using System.Linq;
using System.Linq.Expressions;

namespace CI.TMS.Claim.API.Services
{
    public class ExpenseTypeService : BaseService
    {
        public ExpenseTypeService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<ExpenseTypeService> log)
            : base(context, httpContextAccessor, log)
        {
        }

        public async Task<List<ExpenseTypeResponseDTO>> GetByTAId(string TAId, Expression<Func<ExpenseType, bool>>? predicate = null)
        {
            try
            {
                return await
                context.ExpenseType
                .SelectMany(tat => context.TravelAuthorizationTravelerExtended, (tate, tat) => new { tate = tate, tat = tat })
                .SelectMany(tat => context.TravelAuthorizationTraveler.Where(tate => tat.tat.TATravelerId == tate.Id && tate.TAId == TAId), (tate, tat) => new { tate = tate, tat = tat })
                .Select(select => new { select.tate.tate.ExpenseTypeId, select.tate.tate.ExpenseTypeName, select.tate.tate.AccountCode, select.tate.tate.AccountDescription, select.tate.tate.IsFinance }).Distinct().AsNoTracking().Project().To<ExpenseTypeResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<AccountCodeResponseDTO>> GetListAccountCode()
        {
            try
            {
                var expenseAccount = await context.ExpenseType.GroupBy(x => new { x.AccountCode, x.AccountDescription, x.RuleId, x.IsBankAccount}).Select(x => new AccountCodeResponseDTO { AccountCode = x.Key.AccountCode, AccountDescription = x.Key.AccountDescription, RuleId = x.Key.RuleId, IsBankAccount = x.Key.IsBankAccount }).Distinct().ToListAsync();
                var perdiemAccount = await context.PerdiemType.GroupBy(x => new { x.AccountCode, x.AccountDescription, x.RuleId, x.IsBankAccount}).Select(x => new AccountCodeResponseDTO { AccountCode = x.Key.AccountCode, AccountDescription = x.Key.AccountDescription, RuleId = x.Key.RuleId, IsBankAccount = x.Key.IsBankAccount }).Distinct().ToListAsync();


                var listAccount = expenseAccount.Union(perdiemAccount, new AccountComparer()).OrderBy(x => x.AccountCode);
                var res = listAccount.SelectMany(la => context.ChartOfAccounts.Where(ca => ca.Account == la.AccountCode).DefaultIfEmpty(), (la, ca) => new { la = la, ca = ca }).ToList()
                    .SelectMany(la => context.TravelJournalRule.Where(jr => jr.RuleId == la.la.RuleId).DefaultIfEmpty(), (la, jr) => new { la = la, jr = jr }).ToList()
                    .Select(x => new AccountCodeResponseDTO { 
                        AccountCode = x.la.la.AccountCode, 
                        AccountDescription = x.la.la.AccountDescription, 
                        Type = x.la.ca.Type,
                        Rule = x.jr.Rule,
                        Cat1 = x.jr.Cat1,
                        Cat3 = x.jr.Cat3,
                        Cat4 = x.jr.Cat4,
                        Cat5 = x.jr.Cat5,
                        Cat6 = x.jr.Cat6,
                        Cat7 = x.jr.Cat7,
                        Currency = x.jr.Currency,
                        TaxSystem = x.jr.TaxSystem,
                        IsBankAccount = x.la.la.IsBankAccount == null ? false : x.la.la.IsBankAccount
                    }).ToList();
                return res;


            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
        public async Task<List<ExpenseTypeResponseDTO>> Get(Expression<Func<ExpenseType, bool>>? predicate = null)
        {
            try
            {
                return await context.ExpenseType
                                    .GroupBy(
                                        select => new { select.AccountCode, select.AccountDescription },
                                        (key, group) => new
                                        {
                                            AccountCode = key.AccountCode,
                                            AccountDescription = key.AccountDescription,
                                            ExpenseTypeId = group.First().ExpenseTypeId,
                                            ExpenseTypeName = group.First().ExpenseTypeName,
                                            IsFinance = group.First().IsFinance
                                        }
                                    )
                                    .Select(result => new ExpenseTypeResponseDTO
                                    {
                                        ExpenseTypeId = result.ExpenseTypeId,
                                        ExpenseTypeName = result.ExpenseTypeName,
                                        AccountCode = result.AccountCode,
                                        AccountDescription = result.AccountDescription,
                                        IsFinance = result.IsFinance
                                    }).OrderBy(p => p.AccountDescription)
                                    .AsNoTracking()
                                    .ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public class AccountComparer : IEqualityComparer<AccountCodeResponseDTO>
        {
            public bool Equals(AccountCodeResponseDTO x, AccountCodeResponseDTO y)
            {

                if (Object.ReferenceEquals(x, y)) return true;


                if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                    return false;

                return x.AccountCode == y.AccountCode && x.AccountDescription == y.AccountDescription && x.RuleId == y.RuleId;
            }

            public int GetHashCode(AccountCodeResponseDTO account)
            {
                if (Object.ReferenceEquals(account, null)) return 0;


                return HashCode.Combine(account.AccountCode, account.AccountDescription, account.RuleId);
            }
        }
    }
}
