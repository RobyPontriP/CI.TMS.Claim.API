using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;
using System.Linq;
using System.Linq.Expressions;

namespace CI.TMS.Claim.API.Services
{
    public class CostCenterService : BaseService
    {
        public CostCenterService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<CostCenterService> log)
            : base (context, httpContextAccessor, log) {
        }

        public async Task<List<CostCenterResponseDTO>> Get(string TAId, Expression<Func<CostCenter, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.Id) && x.Status == "Confirmed";

           
                //return await context.CostCenter.Where(predicate).AsNoTracking().Project().To<CostCenterResponseDTO>().ToListAsync();

                var cc = 
                context.CostCenter.Where(predicate)
                .SelectMany(tat => context.TravelAuthorizationCostCenter.Where(tate => tat.Id.ToUpper() == tate.CostCenterId.ToUpper() && tate.TAId == TAId), (tate, tat) => new { tate = tate, tat = tat })
                .Select(select => new { select.tate.Id, select.tate.Name, select.tate.Status}).AsNoTracking().Project().To<CostCenterResponseDTO>().ToListAsync();

  
                if (cc.Result.Count == 0 && await context.TravelAuthorizationSponsorship.Where(s => s.TAId == TAId).CountAsync() > 0)
                {
                  cc = context.CostCenter.Where(predicate).Where(tate => tate.Status == "Confirmed").Select(select => new { select.Id, select.Name, select.Status }).AsNoTracking().Project().To<CostCenterResponseDTO>().ToListAsync();
                }
                return await cc;
            }
            catch(Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}
