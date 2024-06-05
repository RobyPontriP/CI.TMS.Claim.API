using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;
using System.Linq;
using System.Linq.Expressions;

namespace CI.TMS.Claim.API.Services
{
    public class TravelAuthorizationExtendedService : BaseService
    {
        public TravelAuthorizationExtendedService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<TravelAuthorizationExtendedService> log)
            : base(context, httpContextAccessor, log)
        {
        }

        public async Task<List<TravelAuthorizationExtendedResponseDTO>> Get(Expression<Func<TravelAuthorizationExtended, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.TAId);

                return await
                         context.TravelAuthorizationExtended.Where(predicate)
                         .SelectMany(extended => context.TravelAuthorization.Where(ta => extended.TAId == ta.TAId).DefaultIfEmpty(), (extended, ta) => new { ta = ta, extended = extended })
                         .Select(select => new
                         {
                             TotalAdvance = select.extended.TotalAdvance,
                             TACode = select.extended.TACode,
                             ActualAirfare = select.extended.ActualAirfare,
                             OtherActualAirfare =select.extended.OtherActualAirfare,
                             ActualAirfareCur = select.extended.ActualAirfareCur,
                             IsTicketRequired = select.extended.IsTicketRequired,
                             IsHotelRequired = select.extended.IsHotelRequired,
                             IsTravelInsuranceRequired = select.extended.IsTravelInsuranceRequired,
                             Preference = select.extended.Preference,
                             AdditionalInfo = select.ta.Notes,
                             FlightTicket = select.extended.FlightTicket,
                             FlexibleTravelDates = select.extended.FlexibleTravelDates,
                             AdvanceRequired = select.extended.AdvanceRequired,
                             AdvancedRequiredOtherCurrencyIdSecond = select.extended.AdvancedRequiredOtherCurrencyIdSecond,
                             AdvancedRequiredOtherCurrencyPriceSecond = select.extended.AdvancedRequiredOtherCurrencyPriceSecond,
                             AdvancedRequiredOtherCurrencyId = select.ta.AdvancedRequiredOtherCurrencyId,
                             AdvancedRequiredOtherCurrencyPrice = select.ta.AdvancedRequiredOtherCurrencyPrice,
                             AdvanceRequiredDate = select.ta.AdvanceRequiredDate,
                             IsHaveJournalProcessed = (select.ta.IsHaveJournalProcessed == null ? true : select.ta.IsHaveJournalProcessed)
                         }).AsNoTracking().Project().To<TravelAuthorizationExtendedResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<TravelAuthorizationExtendedResponseDTO> UpdateTravelAuthorizationExtended(string id, string action)
        {
            try
            {
                var model = await context.TravelAuthorizationExtended.FirstOrDefaultAsync(x => x.TAId == id);
                if (model == null)
                    throw new Exception("TravelAuthorizationExtended not found.");
                if (action.ToUpper() == "APPROVED")
                {
                    model.ClearCommitmentId = 1;
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





    }
}
