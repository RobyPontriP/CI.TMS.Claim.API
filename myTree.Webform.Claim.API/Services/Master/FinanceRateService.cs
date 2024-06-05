using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;
using System.Linq.Expressions;

namespace CI.TMS.Claim.API.Services
{
    public class FinanceRateService: BaseService
    {
        public FinanceRateService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<BaseService> log)
            : base (context, httpContextAccessor, log) {
        }

        public async Task<List<FinanceRateResponseDTO>> Get(Expression<Func<FinanceRate, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.CurrencyId);

                return await context.FinanceRate.Where(predicate).AsNoTracking().Project().To<FinanceRateResponseDTO>().ToListAsync();
            }catch(Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}
