using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;

namespace CI.TMS.Claim.API.Services
{
    public class TravelJournalService : BaseService
    {
        public static IConfiguration config;

        public TravelJournalService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<TravelJournalService> log, IConfiguration Config)
            : base(context, httpContextAccessor, log)
        {
            config = Config;
        }

        public async Task<List<TravelJournalResponseDTO>> GetByTAId(string taId, Expression<Func<TravelJournal, bool>>? predicate = null)
        {
            try
            {
                var filteredAccount = config["SelectedAccountJournalOCS"].Split('-').ToList();
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.TAId) && x.TAId == taId && filteredAccount.Any(y => y == x.Account);

                return await context.TravelJournal.Where(predicate)
                    .SelectMany(tj => context.PartnerSupplier.Where(emp => emp.Id == tj.AparId).DefaultIfEmpty(), (tj, emp) => new { tj = tj, emp = emp })
                    .SelectMany(tj => context.CostCenter.Where(cc => cc.Id == tj.tj.Cat1).DefaultIfEmpty(), (tj, cc) => new { tj = tj, cc = cc })
                    .SelectMany(tj => context.WorkOrder.Where(wo => wo.Id == tj.tj.tj.Cat4 && wo.CostCenterId == tj.tj.tj.Cat1).DefaultIfEmpty(), (tj, wo) => new { tj = tj, wo = wo })
                    .SelectMany(tj => context.Entity.Where(en => en.Id == tj.tj.tj.tj.Cat7 && en.CostCenterId == tj.tj.tj.tj.Cat1).DefaultIfEmpty(), (tj, en) => new { tj = tj, en = en })
                    .Select(x => new
                    {
                        x.tj.tj.tj.tj.TAId,
                        x.tj.tj.tj.tj.JournalNumber,
                        x.tj.tj.tj.tj.Account,
                        Cat1 = x.tj.tj.cc.Id == null || x.tj.tj.cc.Id == "" ? x.tj.tj.tj.tj.Cat1 : x.tj.tj.cc.Id + " - " + x.tj.tj.cc.Name,
                        x.tj.tj.tj.tj.Cat3,
                        Cat4 = x.tj.wo.Id == null || x.tj.wo.Id == "" ? x.tj.tj.tj.tj.Cat4 : x.tj.wo.Name,
                        x.tj.tj.tj.tj.Cat5,
                        x.tj.tj.tj.tj.Cat6,
                        Cat7 = x.en.Id == null || x.en.Id == "" ? x.tj.tj.tj.tj.Cat7 : x.en.Name,
                        x.tj.tj.tj.tj.TaxSystem,
                        x.tj.tj.tj.tj.Currency,
                        x.tj.tj.tj.tj.Amount,
                        x.tj.tj.tj.tj.AmountUsd,
                        x.tj.tj.tj.tj.Description,
                        x.tj.tj.tj.tj.AparId,
                        AparName = x.tj.tj.tj.emp.Name ?? "",
                        CostCenterId = x.tj.tj.tj.tj.Cat1,
                        WorkOrderId = x.tj.tj.tj.tj.Cat4,
                        EntityId = x.tj.tj.tj.tj.Cat7
                    }).AsNoTracking().Project().To<TravelJournalResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<TravelJournalAdvanceResponseDTO> GetTravelJournalAdvanceByTAId(string taId, Expression<Func<TravelAuthorization, bool>>? predicate = null)
        {
            try
            {
                var journal = await GetByTAId(taId);
                var advanceAmount1 = context.TravelAuthorization.Where(x => !string.IsNullOrEmpty(x.TAId) && x.TAId == taId).Select(x => new AdvanceAmountResponseDTO { Currency = x.AdvancedRequiredOtherCurrencyId, PriceAmount = x.AdvancedRequiredOtherCurrencyPrice }).ToList();
                var advanceAmount2 = context.TravelAuthorizationExtended.Where(x => !string.IsNullOrEmpty(x.TAId) && x.TAId == taId).Select(x => new AdvanceAmountResponseDTO { Currency = x.AdvancedRequiredOtherCurrencyIdSecond, PriceAmount = x.AdvancedRequiredOtherCurrencyPriceSecond }).ToList();

                var listAdvance = advanceAmount1.Union(advanceAmount2).AsEnumerable();

                var advanceAmount = listAdvance.GroupBy(select => select.Currency).Select(x => new AdvanceAmountResponseDTO { Currency = x.Key, PriceAmount = x.Select(y => y.PriceAmount).Sum() }).ToList();
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.TAId) && x.TAId == taId;

                return await context.TravelAuthorization.Where(predicate)
                    .SelectMany(ta => context.TravelAuthorizationExtended.Where(tax => ta.TAId == tax.TAId), (ta, tax) => new { ta = ta, tax = tax })
                    .Select(x => new TravelJournalAdvanceResponseDTO
                    {
                        TAId = x.ta.TAId,
                        TravelJournalList = journal,
                        IsHaveJournalProcessed = (x.ta.IsHaveJournalProcessed == null ? true : x.ta.IsHaveJournalProcessed),
                        AdvanceRequired = (x.tax.AdvanceRequired == 1 ? true : false),
                        AdvanceAmounts = advanceAmount

                    }).AsNoTracking().FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}
