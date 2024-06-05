using CI.TMS.Claim.API.Domain.Entities.Master;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using System.Linq.Expressions;
using System.Linq;
using CI.TMS.Claim.API.Domain.Entities;
using myTree.MicroService.Helper;
using Microsoft.EntityFrameworkCore;

namespace CI.TMS.Claim.API.Services
{
    public class LegalEntityService : BaseService
    {
        public LegalEntityService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<BaseService> log)
            : base(context, httpContextAccessor, log)
        {
        }

        public async Task<List<LegalEntityResponseDTO>> Get(Expression<Func<LegalEntity, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => x.Id != "TBD";

                return await context.LegalEntity.Where(predicate).OrderBy(x=>x.Name)
                    .Project().To<LegalEntityResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}
