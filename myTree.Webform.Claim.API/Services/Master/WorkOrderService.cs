using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;
using System.Linq;
using System.Linq.Expressions;

namespace CI.TMS.Claim.API.Services
{
    public class WorkOrderService : BaseService
    {
        public WorkOrderService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<WorkOrderService> log)
            : base(context, httpContextAccessor, log)
        {
        }

        public async Task<List<WorkOrderResponseDTO>> Get(string TAId, string CostCenterId, Expression<Func<WorkOrder, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.Id) && x.IsActive;//&& Convert.ToDateTime(x.EndDate) > DateTime.Today;
                DateTime datenow = DateTime.Now;
                var woTA = await context.TravelAuthorizationCostCenter
               .SelectMany(tat => context.WorkOrder.Where(tate => tat.WorkOrderId.ToUpper() == tate.Id.ToUpper()).DefaultIfEmpty(), (tate, tat) => new { tate = tate, tat = tat })
               .Select(select => new { select.tate, select.tat })
               .Where(x => x.tate.TAId == TAId && x.tate.CostCenterId == CostCenterId && x.tat.IsActive)
               .Select(select => new WorkOrderResponseDTO
               {
                   Id = select.tate.WorkOrderId,
                   Name = select.tate.WorkOrderName,
                   CostCenterId = select.tate.CostCenterId,
                   CostCenterName = select.tate.CostCenterName,
                   EndDate = select.tat.EndDate
               })
                 .AsNoTracking()
                 .ToListAsync();

                if (woTA.Count == 0 && await context.TravelAuthorizationSponsorship.Where(s => s.TAId == TAId).CountAsync() > 0)
                {
                    woTA = await context.WorkOrder
                   .Where(x => x.CostCenterId == CostCenterId && x.IsActive)
                   .Select(select => new WorkOrderResponseDTO
                   {
                       Id = select.Id,
                       Name = select.Name,
                       CostCenterId = select.CostCenterId,
                       CostCenterName = select.CostCenterName,
                       EndDate = select.EndDate
                   })
                     .AsNoTracking()
                     .ToListAsync();
                }
                
                
                return woTA.Distinct(new WorkOrderResponseDTOComparer()).ToList();


                #region WO TA and Project Related
                //var listProjectId = context.WorkOrder.Where(x => !string.IsNullOrEmpty(x.Id) && x.IsActive == "1")
                //                .SelectMany(tat => context.TravelAuthorizationCostCenter.Where(tate => tat.Id.ToUpper() == tate.WorkOrderId.ToUpper()
                //                && tate.TAId == TAId ).Where(x => x.TAId == TAId)
                //                .Distinct(), (tate, tat) => new { tate = tate, tat = tat })
                //                .Select(select => new { select.tate.ProjectId }).AsEnumerable();


                // var woTA = await context.TravelAuthorizationCostCenter
                //.SelectMany(tat => context.WorkOrder.Where(tate => tat.WorkOrderId.ToUpper() == tate.Id.ToUpper()).DefaultIfEmpty(), (tate, tat) => new { tate = tate, tat = tat })
                //.Select(select => new { select.tate, select.tat })
                //.Where(x => x.tate.TAId == TAId && x.tate.CostCenterId == CostCenterId)
                //.Select(select => new WorkOrderResponseDTO
                //  {
                //      Id = select.tate.WorkOrderId,
                //      Name = select.tate.WorkOrderName,
                //      CostCenterId = select.tate.CostCenterId,
                //      CostCenterName = select.tate.CostCenterName,
                //      EndDate =select.tat.EndDate
                //  })
                //  .AsNoTracking()
                //  .ToListAsync();

                //// Project data into WorkOrderResponseDTO
                //var data = await context.WorkOrder
                //    .Where(predicate)
                //    .Where(x => listProjectId.Any(y => y.ProjectId == x.ProjectId))
                //    .Select(select => new WorkOrderResponseDTO
                //    {
                //        Id = select.Id,
                //        Name = select.Name,
                //        CostCenterId = select.CostCenterId,
                //        CostCenterName = select.CostCenterId + " - " + select.CostCenterName,
                //        EndDate = select.EndDate
                //    })
                //    .Where(x => x.CostCenterId == CostCenterId && x.EndDate > datenow)
                //    .OrderBy(x => x.Id)
                //    .ToListAsync();

                //var combinedResults = woTA.Union(data, new WorkOrderResponseDTOComparer());

                //return combinedResults.Distinct(new WorkOrderResponseDTOComparer()).ToList();
                #endregion

            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public class WorkOrderResponseDTOComparer : IEqualityComparer<WorkOrderResponseDTO>
        {
            public bool Equals(WorkOrderResponseDTO x, WorkOrderResponseDTO y)
            {
                if (x == null || y == null)
                    return false;

                return x.Id == y.Id && x.Name == y.Name && x.CostCenterId == y.CostCenterId && x.CostCenterName == y.CostCenterName && x.EndDate == y.EndDate;
            }

            public int GetHashCode(WorkOrderResponseDTO obj)
            {
                return HashCode.Combine(obj.Id, obj.Name, obj.CostCenterId, obj.CostCenterName, obj.EndDate);
            }
        }
    }
}
