using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;
using System.Linq.Expressions;

namespace CI.TMS.Claim.API.Services
{
    public class CurrencyService: BaseService
    {
        public CurrencyService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<CurrencyService> log)
            : base (context, httpContextAccessor, log) {
        }

        public async Task<List<CurrencyResponseDTO>> Get(Expression<Func<Currency, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.CurrencyCode);

                return await context.Currency.Where(predicate).AsNoTracking().Project().To<CurrencyResponseDTO>().ToListAsync();
            }catch(Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}
