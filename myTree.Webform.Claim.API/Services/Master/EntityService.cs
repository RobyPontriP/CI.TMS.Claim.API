using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;
using System.Linq;
using System.Linq.Expressions;

namespace CI.TMS.Claim.API.Services
{
    public class EntityService : BaseService
    {
        public EntityService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<EntityService> log)
            : base(context, httpContextAccessor, log)
        {
        }

        public async Task<List<EntityResponseDTO>> Get(string TAId, string CostCenterId, Expression<Func<Entity, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.Id) && x.IsActive == "1";//&& Convert.ToDateTime(x.EndDate) > DateTime.Today;

                var ett = context.Entity.Where(predicate)
                          .SelectMany(tat => context.TravelAuthorizationCostCenter.Where(tate => tat.CostCenterId.ToUpper() == tate.CostCenterId.ToUpper()
                          && tat.Id == tate.EntityId && tate.TAId == TAId), (tate, tat) => new { tate = tate, tat = tat })
                          .Select(select => new
                          { select.tate.Id, select.tate.Name, select.tate.CostCenterId, select.tate.LegalEntityId })
                          .Where(x => x.CostCenterId == CostCenterId)
                          .Distinct().AsNoTracking().Project().To<EntityResponseDTO>().ToListAsync();

                if (ett.Result.Count == 0 && await context.TravelAuthorizationSponsorship.Where(s => s.TAId == TAId).CountAsync() > 0)
                {
                    ett = GetEntityByCostCenterId(CostCenterId);
                }

                return await ett;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<EntityResponseDTO>> GetEntityByCostCenterId(string CostCenterId, Expression<Func<Entity, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.Id) && x.IsActive == "1";

                var ett = await context.Entity.Where(predicate)
                          .Select(select => new
                          { select.Id, select.Name, select.CostCenterId, select.LegalEntityId })
                          .Where(x => x.CostCenterId == CostCenterId)
                          .Distinct().AsNoTracking().Project().To<EntityResponseDTO>().ToListAsync();

                return ett;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}
