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
    public class TravelAuthorizationDestinationService : BaseService
    {
        public TravelAuthorizationDestinationService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<TravelAuthorizationDestinationService> log)
            : base (context, httpContextAccessor, log) {
        }

        public async Task<List<TravelAuthorizationDestinationResponseDTO>> Get(Expression<Func<TravelAuthorizationDestination, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.Id);

                return await context.TravelAuthorizationDestination.Where(predicate).AsNoTracking().Project().To<TravelAuthorizationDestinationResponseDTO>().ToListAsync();
            }catch(Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<TravelAuthorizationTripDateResponseDTO>> GetStartDateTrip(Expression<Func<TravelAuthorizationDestination, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.Id) ;

                return await context.TravelAuthorizationDestination.Where(predicate).OrderBy(p => p.StartDate).AsNoTracking().Project().To<TravelAuthorizationTripDateResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<TravelAuthorizationTripDateResponseDTO>> GetEndDateTrip(Expression<Func<TravelAuthorizationDestination, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.Id);

                return await context.TravelAuthorizationDestination.Where(predicate).OrderByDescending(p => p.EndDate).AsNoTracking().Project().To<TravelAuthorizationTripDateResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<TravelAuthorizationCountHRResponseDTO>> GetCountHR(string TAId,Expression<Func<TravelAuthorizationDestination, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.Id);

                var queryResult = context.TravelAuthorizationDestination
                .Where(td => td.TypeOfTravel != null && td.TAId == TAId)
                .GroupBy(td => td.TypeOfTravel)
                .Select(group => new
                {
                    TypeOfTravel = group.Key,
                    CountResult = group.Count()
                }).ToList();

                decimal tes = queryResult.Distinct().Select(p => p.CountResult).Count();
                return await
                         context.TravelAuthorizationDestination.Where(x=> x.TAId == TAId)
                         .Select(select => new
                         {
                             CountResult = (queryResult.Distinct().Select(p => p.CountResult).Count() == 1 && queryResult.Select(p => p.TypeOfTravel).FirstOrDefault().ToLower() == "hr related" ? "Only HR" : "Mixed"),
                             TAId = select.TAId
                         }).Take(1).AsNoTracking().Project().To<TravelAuthorizationCountHRResponseDTO>().ToListAsync();

                //var result = context.TravelAuthorization
                //    .Select(ta => new
                //    {
                //        CountResult = context.TravelAuthorizationDestination
                //            .Where(td => td.TypeOfTravel != null && td.TAId == ta.TAId)
                //            .GroupBy(td => ta.TAId)
                //            .Select(group => new
                //            {
                //                CountResult = ""
                //            })
                //            .FirstOrDefault(),
                //        ta.TAId // You can include other properties from TravelAuthorization if needed
                //    }).
                //    Where(x => x.TAId == TAId).AsNoTracking().Project().To<TravelAuthorizationCountHRResponseDTO>().ToListAsync();
                //return await result;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<TravelAuthorizationCountPersonalTravelResponseDTO>> GetCountPersonalTravel (string TAId, Expression<Func<TravelAuthorizationDestination, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.Id);


                var queryResult = context.TravelAuthorizationDestination
                .Where(td => td.TypeOfTravel != null && td.TypeOfTravel == "Personal travel" && td.TAId == TAId)
                .Select(select => new
                {
                    CountResult = select.TypeOfTravel
                    ,TAId = select.TAId
                }
                ).AsNoTracking().ToList();

                return await
                              context.TravelAuthorizationDestination.Where(x => x.TAId == TAId)
                              .Select(select => new
                              {
                                  CountResult = queryResult.Count() == 0 ? "0" : queryResult.Select(p => p.CountResult).Count().ToString(),
                                  TAId = select.TAId
                              }).AsNoTracking().Project().To<TravelAuthorizationCountPersonalTravelResponseDTO>().ToListAsync();
               

            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}
