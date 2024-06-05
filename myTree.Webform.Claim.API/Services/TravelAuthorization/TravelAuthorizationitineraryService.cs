using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;
using System.Linq;
using System.Linq.Expressions;

namespace CI.TMS.Claim.API.Services
{
    public class TravelAuthorizationItineraryService : BaseService
    {
        public TravelAuthorizationItineraryService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<TravelAuthorizationItineraryService> log)
            : base (context, httpContextAccessor, log) {
        }

        public async Task<List<TravelAuthorizationItineraryResponseDTO>> Get(Expression<Func<TravelAuthorizationItinerary, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => (x.Id != null);

                return await
              context.TravelAuthorizationItinerary.Where(predicate)
              .SelectMany(itinerary => context.Airports.Where(airport1 => itinerary.AirportIdFrom == airport1.Id).DefaultIfEmpty(), (itinerary, airport1) => new { itinerary = itinerary, airport1 = airport1 })
              .SelectMany(itinerary => context.Airports.Where(airport2 => itinerary.itinerary.AirportIdTo == airport2.Id).DefaultIfEmpty(), (itinerary, airport2) => new { itinerary = itinerary, airport2 = airport2 })
              .SelectMany(itinerary => context.TravelAuthorizationExtended.Where(extended => itinerary.itinerary.itinerary.TAId == extended.TAId).DefaultIfEmpty(), (itinerary, extended) => new { itinerary = itinerary, extended = extended })
              .Select(select => new {
                  Id = select.itinerary.itinerary.itinerary.Id,
                  TAId = select.itinerary.itinerary.itinerary.TAId,
                  AirportIdFrom = select.itinerary.itinerary.itinerary.AirportIdFrom,
                  AirportFrom = "",
                  AirportOtherFrom = select.itinerary.itinerary.itinerary.AirportOtherFrom,
                  CityFrom = select.itinerary.itinerary.airport1.City,
                  IataFrom = select.itinerary.itinerary.airport1.Iata,
                  NameFrom = select.itinerary.itinerary.airport1.Name,
                  CountryFrom = select.itinerary.itinerary.airport1.Country,
                  CityTo = select.itinerary.airport2.City,
                  IataTo = select.itinerary.airport2.Iata,
                  NameTo = select.itinerary.airport2.Name,
                  CountryTo = select.itinerary.airport2.Country,
                  AirportIdTo = select.itinerary.itinerary.itinerary.AirportIdTo,
                  AirportTo = "",
                  AirportOtherTo = select.itinerary.itinerary.itinerary.AirportOtherTo,
                  ClassOfTravel = select.itinerary.itinerary.itinerary.ClassOfTravel,
                  TravelDate = select.itinerary.itinerary.itinerary.TravelDate,
                  TravelDateLabel = "",
                  TravelTime = select.itinerary.itinerary.itinerary.TravelTime,
                  TravelTTimeTypeime = select.itinerary.itinerary.itinerary.TimeType,
                  TimeTypeLabel = "",
                  Remarks = select.itinerary.itinerary.itinerary.Remarks,
                  IsActive = select.itinerary.itinerary.itinerary.IsActive,
                  TravelType = select.itinerary.itinerary.itinerary.TravelType,
                  IsTicketRequired = select.extended.IsTicketRequired,
              }).OrderBy(x => x.TravelDate).OrderBy(x => x.TravelTime).AsNoTracking().Project().To<TravelAuthorizationItineraryResponseDTO>().ToListAsync();

                // return await context.TravelAuthorizationItinerary.Where(predicate).AsNoTracking().Project().To<TravelAuthorizationItineraryResponseDTO>().ToListAsync();


            }
            catch(Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}
