using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;
using System.Linq.Expressions;

namespace CI.TMS.Claim.API.Services
{
    public class MasterIKICostCentersService : BaseService
    {
        public MasterIKICostCentersService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<MasterIKICostCentersService> log)
            : base (context, httpContextAccessor, log) {
        }


        public async Task<List<MasterIKICostCentersResponseDTO>> Get(Expression<Func<MasterIKICostCenters, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.ProjectId);

                return await context.MasterIKICostCenters.Where(predicate).Select(p => new { ProjectId = p.ProjectId, PerdiemType = p.PerdiemType }).OrderBy(p => p.ProjectId).AsNoTracking().Project().To<MasterIKICostCentersResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }


    }
}
