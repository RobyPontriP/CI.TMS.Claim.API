using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;
using System.Linq;
using System.Linq.Expressions;

namespace CI.TMS.Claim.API.Services
{
    public class TravelAuthorizationCostCenterService : BaseService
    {
        public TravelAuthorizationCostCenterService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<TravelAuthorizationCostCenterService> log)
            : base (context, httpContextAccessor, log) {
        }

        public async Task<List<TravelAuthorizationCostCenterResponseDTO>> Get(Expression<Func<TravelAuthorizationCostCenter, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => x.CostCenterAmount > 0;


                return await
                context.TravelAuthorizationCostCenter.Where(predicate)
                .SelectMany(tat => context.CostCenter.Where(cc => cc.Id == tat.CostCenterId).DefaultIfEmpty(), (tat, cc) => new { Tat = tat, CC = cc})
                .SelectMany(tat => context.WorkOrder.Where(wo => wo.CostCenterId == tat.Tat.CostCenterId && wo.Id == tat.Tat.WorkOrderId).DefaultIfEmpty(), (tat, wo) => new { Tat = tat, WO = wo})
                .SelectMany(tat => context.Entity.Where(et => et.CostCenterId == tat.Tat.Tat.CostCenterId && et.Id == tat.Tat.Tat.EntityId).DefaultIfEmpty(), (tat, et) => new { Tat = tat, ET = et})
                .Select(select => new 
                { 
                    Id = select.Tat.Tat.CC.Id, 
                    Percentage = select.Tat.Tat.Tat.Percentage, 
                    Remarks = select.Tat.Tat.Tat.Remarks,
                    TAId = select.Tat.Tat.Tat.TAId,
                    CostCenterId = select.Tat.Tat.CC.Id,
                    CostCenterName = select.Tat.Tat.CC.Id + " - " + select.Tat.Tat.CC.Name,
                    WorkOrderId = select.Tat.WO.Id,
                    WorkOrderName = select.Tat.WO.Name,
                    EntityId = select.ET.Id,
                    EntityName = select.ET.Name,
                    CostCenterAmount = select.Tat.Tat.Tat.CostCenterAmount,
                    CostCenterAmounUSD = select.Tat.Tat.Tat.CostCenterAmounUSD,
                    LegalEntityId = select.Tat.Tat.Tat.LegalEntityId
                }).OrderBy(p=> p.CostCenterId).AsNoTracking().Project().To<TravelAuthorizationCostCenterResponseDTO>().ToListAsync();
                //return await context.TravelAuthorizationCostCenter.Where(predicate).AsNoTracking().Project().To<TravelAuthorizationCostCenterResponseDTO>().ToListAsync();
            }catch(Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}
