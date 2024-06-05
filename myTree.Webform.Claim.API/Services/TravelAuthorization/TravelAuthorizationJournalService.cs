using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;

namespace CI.TMS.Claim.API.Services
{
    public class TravelAuthorizationJournalService : BaseService
    {
        public TravelAuthorizationJournalService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<TravelAuthorizationJournalService> log)
           : base(context, httpContextAccessor, log)
        {
        }

        public async Task<List<TravelAuthorizationJournalResponseDTO>> Get(Expression<Func<TravelAuthorizationJournal, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => x.Id != Guid.Empty;

                return await context.TravelAuthorizationJournal.Where(predicate).AsNoTracking().Project().To<TravelAuthorizationJournalResponseDTO>().ToListAsync();

            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}
