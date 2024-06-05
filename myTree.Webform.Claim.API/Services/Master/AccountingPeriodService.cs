using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using System.Linq.Expressions;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;

namespace CI.TMS.Claim.API.Services
{
    public class AccountingPeriodService : BaseService
    {
        public AccountingPeriodService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<AccountingPeriodService> log)
          : base(context, httpContextAccessor, log)
        {
        }

        public async Task<List<AccountingPeriodResponseDTO>> Get(Expression<Func<AccountingPeriod, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => (x.Id.Substring(x.Id.Length - 2)) != "13" && x.Status == "N";

                return await context.AccountingPeriod.Where(predicate).OrderByDescending(x => x.Id).AsNoTracking().Project().To<AccountingPeriodResponseDTO>().ToListAsync();

            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}
