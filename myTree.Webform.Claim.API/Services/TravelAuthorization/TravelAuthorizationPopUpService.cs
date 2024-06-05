using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;
using System.Linq;
using System.Linq.Expressions;

namespace CI.TMS.Claim.API.Services
{
    public class TravelAuthorizationPopUpService : BaseService
    {
        public TravelAuthorizationPopUpService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<TravelAuthorizationPopUpService> log)
            : base(context, httpContextAccessor, log)
        {
        }

        public async Task<List<TravelAuthorizationPopUpResponseDTO>> Get(Expression<Func<TravelAuthorizationPopUp, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.Destination);

                return await context.TravelAuthorizationPopUp.Where(predicate).AsNoTracking().Project().To<TravelAuthorizationPopUpResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<TravelAuthorizationPopUpResponseDTO>> GetTravelAuthorizationPopUpByUserId(string userid, Expression<Func<TravelAuthorizationPopUp, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.Destination);

                return await context.TravelAuthorizationPopUp.Where(x => (x.TravelerId == userid || x.CreatedBy == userid)).AsNoTracking().Project().To<TravelAuthorizationPopUpResponseDTO>().ToListAsync();
                //return await
                //   context.TravelAuthorization.Where(predicate)
                //   .SelectMany(a => context.TravelAuthorizationTraveler.Where(b => a.TAId == b.TAId).DefaultIfEmpty(), (a, b) => new { Traveler = b, Travel = a })
                //   .SelectMany(b => context.Employee.Where(d => d.RowId == b.Traveler.Id).DefaultIfEmpty(), (b, d) => new { Traveler = b, Employee = d })
                //   .Select(select => new { 
                //   select.Traveler.Traveler.TravelerName, select.Traveler.Travel.CreatedBy, select.Employee.EmpName
                //   }).AsNoTracking().Project().To<TravelAuthorizationPopUpResponseDTO>().ToListAsync();


            }

            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}
