using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;
using System.Linq.Expressions;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace CI.TMS.Claim.API.Services
{
    public class ClaimConditionService: BaseService
    {
        public ClaimConditionService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<ClaimConditionService> log)
            : base (context, httpContextAccessor, log) {
        }

        public async Task<List<ClaimConditionResponseDTO>> Get(Expression<Func<ClaimCondition, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.Description);

                return await context.ClaimCondition.Where(predicate).AsNoTracking().Project().To<ClaimConditionResponseDTO>().ToListAsync();
            }
            catch(Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

    }
}
