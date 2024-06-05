using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using myTree.MicroService.Helper;
using System.Linq;
using System.Linq.Expressions;

namespace CI.TMS.Claim.API.Services
{
    public class TravelAuthorizationService : BaseService
    {
        TravelAuthorizationDestinationService travelauthorizationdestinationSvc;
        TravelAuthorizationTravelerService travelauthorizationtravelerSvc;
        TravelAuthorizationCostCenterService travelauthorizationcostcenterSvc;
        TravelAuthorizationExtendedService travelauthorizationextendedSvc;

        public TravelAuthorizationService(
            ClaimContext _context,
            IHttpContextAccessor _httpContextAccessor,
            ILogger<BaseService> _log,
            TravelAuthorizationDestinationService _travelauthorizationdestinationSvc,
            TravelAuthorizationTravelerService _travelauthorizationtravelerSvc,
            TravelAuthorizationCostCenterService _travelauthorizationcostcenterSvc,
            TravelAuthorizationExtendedService _travelauthorizationextendedSvc
            ) : base(_context, _httpContextAccessor, _log)
        {
            travelauthorizationdestinationSvc = _travelauthorizationdestinationSvc;
            travelauthorizationtravelerSvc = _travelauthorizationtravelerSvc;
            travelauthorizationcostcenterSvc = _travelauthorizationcostcenterSvc;
            travelauthorizationextendedSvc = _travelauthorizationextendedSvc;
        }
        public async Task<TravelAuthorizationDataResponseDTO> GetAllData(string requestId)
        {
            try
            {
                TravelAuthorizationDataResponseDTO ret = new TravelAuthorizationDataResponseDTO();

                var taDestination = await travelauthorizationdestinationSvc.Get(x => x.TAId == requestId);
                var taCostCenter = await travelauthorizationcostcenterSvc.Get(x => x.TAId == requestId);
                var taTraveler = await travelauthorizationtravelerSvc.Get(x => x.TAId == requestId);
                var taStartTripDate = await travelauthorizationdestinationSvc.GetStartDateTrip(x => x.TAId == requestId);
                var taEndTripDate = await travelauthorizationdestinationSvc.GetEndDateTrip(x => x.TAId == requestId);
                var taTotalAdvance = await travelauthorizationextendedSvc.Get(x => x.TAId == requestId);

                ret.Destination = taDestination;
                ret.ChargeCode = taCostCenter;
                ret.TravelerType = taTraveler.Select(p => p.TravelerType).FirstOrDefault();
                ret.TravelerName = taTraveler.Select(p => p.TravelerName).FirstOrDefault();
                ret.StartDate = taStartTripDate.Select(p => p.StartDate).FirstOrDefault();
                ret.EndDate = taEndTripDate.Select(p => p.EndDate).FirstOrDefault();

                return ret;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<TravelAuthorizationDataResponseDTO>> Get(Expression<Func<TravelAuthorization, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.TAId);

                return await context.TravelAuthorization.Where(predicate).AsNoTracking().Project().To<TravelAuthorizationDataResponseDTO>().ToListAsync();

            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<TravelAuthorizationDataResponseDTO> UpdateAlreadyHaveExpensePerdiemClaim(string id, string action)
        {
            try
            {
                var model = await context.TravelAuthorization.FirstOrDefaultAsync(x => x.TAId == id);
                if (model == null)
                    throw new Exception("TravelAuthorization not found.");
                if (action.ToUpper() == "SUBMITTED" || action.ToUpper() == "APPROVED")
                {
                    model.IsAlreadyHaveExpenseClaim = true;
                    model.IsAlreadyHavePerdiemClaim = true;
                }
                else
                {
                    model.IsAlreadyHaveExpenseClaim = false;
                    model.IsAlreadyHavePerdiemClaim = false;
                }

                context.Update(model);
                await context.SaveChangesAsync();
                return (await this.Get(predicate: (x => x.TAId == model.TAId))).FirstOrDefault();

            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<TravelAuthorizationCarbonOffsetResponseDTO>> GetTravelCarbonOffsetByTAId(string taid, Expression<Func<TravelAuthorization, bool>>? predicate = null)
        {
            try
            {

                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.TAId) && x.TAId == taid && x.AmountTravel != 0 && x.AmountTravel != null;

                return await context.TravelAuthorization.Where(predicate)
                    .SelectMany(ta => context.TravelAuthorizationCarbonOffset.Where(tc => tc.TAId == ta.TAId).DefaultIfEmpty(), (ta, tc) => new { ta = ta, tc = tc })
                    .Select(select => new TravelAuthorizationCarbonOffsetResponseDTO
                    {
                        TAId = select.ta.TAId,
                        TravelCategory = select.tc.TravelCategory,
                        AmountTravel = select.tc.AmountTravel,
                        JournalAccountCarbon = select.tc.JournalAccountCarbon,
                        JournalAccountBalancer = select.tc.JournalAccountBalancer,
                        AparId = select.tc.AparId,
                        CostCenterId = select.tc.CostCenterId,
                        WorkOrderId = select.tc.WorkOrderId,
                        EntityId = select.tc.EntityId,
                        LegalEntityId = select.tc.LegalEntityId,
                        Percentage = select.tc.Percentage,
                        CostCenterAmount = select.tc.CostCenterAmount,
                        CostCenterAmountUSD = select.tc.CostCenterAmountUSD

                    }).OrderBy(x=>x.CostCenterId).AsNoTracking().ToListAsync();

            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}