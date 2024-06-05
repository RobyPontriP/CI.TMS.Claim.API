using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Request;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;
using System.Linq;
using System.Linq.Expressions;

namespace CI.TMS.Claim.API.Services
{
    public class TravelAuthorizationSponsorshipService : BaseService
    {
        public TravelAuthorizationSponsorshipService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<TravelAuthorizationSponsorshipService> log)
            : base (context, httpContextAccessor, log) {
        }

        public async Task<List<TravelAuthorizationSponsorshipResponseDTO>> Get(Expression<Func<TravelAuthorizationSponsorship, bool>>? predicate = null)
        {
            try
            {

                return await context.TravelAuthorizationSponsorship.AsNoTracking().Project().To<TravelAuthorizationSponsorshipResponseDTO>().ToListAsync();
            }catch(Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        

        public async Task<List<TravelAuthorizationCountSponsorshipResponseDTO>> GetCountSponsorship(string TAId,Expression<Func<TravelAuthorizationSponsorship, bool>>? predicate = null)
        {
            try
            {

                var queryResult = context.TravelAuthorizationSponsorship
                .Where(td =>  td.TAId == TAId)
                .GroupBy(td => td.TAId)
                .Select(group => new
                {
                    TAId = group.Key,
                    CountResult = Convert.ToInt32(group.Sum(td => td.TypeValue))
                }).ToList();

                decimal tes = queryResult.Distinct().Select(p => p.CountResult).Count();
                return await
                         context.TravelAuthorizationSponsorship.Where(x=> x.TAId == TAId)
                         .Select(select => new
                         {
                             CountResult = Convert.ToInt32(queryResult.Select(p => p.CountResult).FirstOrDefault()),
                             TAId = select.TAId
                         }).Take(1).AsNoTracking().Project().To<TravelAuthorizationCountSponsorshipResponseDTO>().ToListAsync();

              
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

    }
}
