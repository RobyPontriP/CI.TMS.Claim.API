using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;
using System.Linq;
using System.Linq.Expressions;

namespace CI.TMS.Claim.API.Services
{
    public class TravelAuthorizationTravelerService : BaseService
    {
        public TravelAuthorizationTravelerService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<TravelAuthorizationTravelerService> log)
            : base(context, httpContextAccessor, log)
        {
        }

        public async Task<List<TravelAuthorizationTravelerResponseDTO>> Get(Expression<Func<TravelAuthorizationTraveler, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.TAId);

                return await 
                context.TravelAuthorizationTraveler.Where(predicate).AsNoTracking().Project().To<TravelAuthorizationTravelerResponseDTO>().ToListAsync();

            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<TravelAuthorizationTravelerResponseDTO>> GetByTAId(Expression<Func<TravelAuthorizationTraveler, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.TAId);

                return await
                context.TravelAuthorizationTraveler.Where(predicate)
                .SelectMany(tat => context.TravelAuthorizationTravelerExtended.Where(tate => tat.Id.ToUpper() == tate.TATravelerId.ToUpper()).DefaultIfEmpty(), (tate, tat) => new { tate = tate, tat = tat })
                .SelectMany(tat => context.Employee.Where(emp => tat.tate.TravelerId.ToUpper() == emp.RowId.ToUpper()).DefaultIfEmpty(), (emp, tat) => new { emp = emp, tat = tat })
                .SelectMany(tat => context.DutyPost.Where(dpt => tat.emp.tat.TravelerDutyPostCode.ToUpper() == dpt.TravelerDutyPostCode.ToUpper()).DefaultIfEmpty(), (dpt, tat) => new { dpt = dpt, tat = tat })
                .SelectMany(tat => context.PartnerSupplier.Where(supp => tat.dpt.emp.tate.TravelerId == supp.Id).DefaultIfEmpty(), (supp, tat) => new { supp = supp, tat = tat })
                .Select(select => new {
                    select.supp.dpt.emp.tate.TravelerId,
                    select.supp.dpt.emp.tate.TravelerName,
                    select.supp.dpt.emp.tate.DirectSpv,
                    select.supp.dpt.emp.tate.Id,
                    select.supp.dpt.emp.tate.TAId,
                    select.supp.dpt.emp.tat.TravelerType,
                    select.supp.dpt.emp.tat.ParticipantLetter,
                    select.supp.dpt.emp.tat.DetailTraveler,
                    select.supp.dpt.tat.Gender,
                    select.supp.dpt.emp.tat.WorkingLocation,
                    select.supp.tat.TravelerDutyPostName,
                    select.tat.TaxSystem,
                    AparId = select.supp.dpt.emp.tate.TravelerId,
                }).AsNoTracking().Project().To<TravelAuthorizationTravelerResponseDTO>().ToListAsync();

            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}
