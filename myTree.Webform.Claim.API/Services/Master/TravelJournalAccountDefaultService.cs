using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using System.Linq.Expressions;
using System.Linq;
using myTree.MicroService.Helper;
using Microsoft.EntityFrameworkCore;

namespace CI.TMS.Claim.API.Services
{
    public class TravelJournalAccountDefaultService : BaseService
    {
        public TravelJournalAccountDefaultService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<BaseService> log)
            : base(context, httpContextAccessor, log)
        {
        }

        public async Task<List<TravelJournalAccountDefaultResponseDTO>> Get(Expression<Func<Domain.Entities.TravelJournalAccountDefault, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id != "" && x.IsActive;
                return await context.TravelJournalAccountDefault.Where(predicate).Project().To<TravelJournalAccountDefaultResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}
